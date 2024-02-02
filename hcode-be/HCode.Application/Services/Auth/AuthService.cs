using AutoMapper;
using HCode.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HCode.Application
{
    /// <summary>
    /// Triển khai service auth
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class AuthService : CoreService, IAuthService
    {
        #region Fields
        /// <summary>
        /// Service gửi email
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// Repo auth
        /// </summary>
        private readonly IAccountRepository _repository;
        /// <summary>
        /// Repo vai trò
        /// </summary>
        private readonly IRoleRepository _roleRepository;
        /// <summary>
        /// Cache
        /// </summary>
        /// Created by: nlnhat (13/07/2023
        protected readonly IMemoryCache _cache;
        /// <summary>
        /// Jwt setting
        /// </summary>
        private readonly JwtConfig _jwtConfig;
        /// <summary>
        /// Auth config
        /// </summary>
        private readonly AuthConfig _authConfig;
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service
        /// </summary>
        /// <param name="repository">Repository account</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map đối tượng</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public AuthService(IAccountRepository repository, IRoleRepository roleRepository,
                           IStringLocalizer<Resource> resource, IMapper mapper, IEmailService emailService, IMemoryCache cache,
                           IOptions<JwtConfig> jwtConfig, IOptions<AuthConfig> authConfig,
                           IHttpContextAccessor httpContext)
                         : base(resource, mapper)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _emailService = emailService;
            _cache = cache;
            _jwtConfig = jwtConfig.Value;
            _authConfig = authConfig.Value;
            _httpContext = httpContext;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validate tạo mới tài khoản
        /// </summary>
        /// <param name="authDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private async Task ValidateCreateAccountAsync(AuthDto authDto, ServerResponse res)
        {
            var existedAccount = await _repository.GetByUsernameAsync(authDto.UserName);

            // Nếu đã tồn tại Username dùng cho account khác
            if (existedAccount != null && existedAccount.AccountId != authDto.AccountId)
            {
                res.OnError
                (
                    ErrorCode.AuthExistedUsername,
                    new ErrorItem("refUsername", _resource["AuthExistedUsername"])
                );
            }
        }
        
        // Đăng ký
        public async Task SignupAsync(AuthDto authDto, ServerResponse res)
        {
            await ValidateCreateAccountAsync(authDto, res);

            if (res.Success)
            {
                //var role = await _roleRepository.GetByCodeAsync(RoleConstant.RoleCode.Admin);
                //var roleId = (role != null) ? role.RoleId : Guid.Empty;

                var (password, salt) = AppHelper.HashPassword(authDto.Password);

                var accountId = Guid.NewGuid();

                var account = new Account()
                {
                    AccountId = accountId,
                    Username = authDto.UserName,
                    Password = password,
                    Email = authDto.Email ?? string.Empty,
                    Salt = salt,
                    Role = RoleCode.User,
                    CreatedTime = DateTime.UtcNow,
                    CreatedBy = accountId.ToString(),
                };

                var insertRes = await _repository.InsertAsync(account);

                if (insertRes != Guid.Empty)
                {
                    await SendVerifyCodeAsync(authDto, res);
                }
                else
                {
                    res.OnError(ErrorCode.AuthVerify, _resource["AuthSignupError"]);
                }
            }
        }

        // Gửi mã xác thực
        public async Task<string?> SendVerifyCodeAsync(AuthDto authDto, ServerResponse res)
        {
            if (!string.IsNullOrWhiteSpace(authDto.Email))
            {
                var random = new Random();
                var randomNumber = random.Next(0, _authConfig.MaxVerifyCode);
                var verifyCode = randomNumber.ToString($"D{_authConfig.PaddingVerifyCode}");
                var expireTime = _authConfig.VerifyTimeOut;

                var verifyModeContent = authDto.VerifyMode switch
                {
                    VerifyMode.ChangePassword => _resource["AuthChangePassword"],
                    VerifyMode.ChangeEmail => _resource["AuthChangeEmail"],
                    _ => _resource["AuthSignup"]
                };

            var message = new EmailMessage()
                {
                    To = authDto.Email,
                    Subject = _resource["AuthSubjectVerifyEmail"],
                    Content = string.Format(
                        _resource["AuthContentVerifyEmail"],
                        verifyModeContent,
                        $"<b>{authDto.UserName}</b>",
                        $"<b style=\"color: #00f\">{verifyCode}</b>",
                        $"<b>{expireTime}</b>")
                };

                var sendResult = await _emailService.SendEmailAsync(message);

                res.Success = sendResult.Success;
                res.DevMsg = sendResult.DevMsg;

                if (!res.Success)
                {
                    res.OnError(ErrorCode.AuthVerifyEmail, _resource["AuthVerifyEmail"]);
                }
                else
                {
                    var options = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(expireTime)
                    };

                    _cache.Set($"Username_{authDto.UserName}", verifyCode, options);
                }

                return verifyCode;
            }
            else
            {
                res.OnError(ErrorCode.AuthVerifyEmail, _resource["AuthVerifyEmail"]);
                return null;
            }
        }

        // Xác thực
        public async Task VerifyAsync(AuthDto authDto, ServerResponse res)
        {
            if (_cache.TryGetValue($"Username_{authDto.UserName}", out string? cacheVerifyCode))
            {
                if (authDto.VerifyCode == cacheVerifyCode)
                {
                    res.OnSuccess();
                }
                else
                {
                    res.OnError
                    (
                        ErrorCode.AuthIncorrectVerifyCode,
                        new ErrorItem("refVerifyCode", _resource["AuthIncorrectVerifyCode"])
                    );
                }
            }
            else
            {
                res.OnError
                (
                    ErrorCode.AuthIncorrectVerifyCode,
                    new ErrorItem("refVerifyCode", _resource["AuthIncorrectVerifyCode"])
                );
            }
        }

        // Đăng nhập
        public async Task LoginAsync(AuthDto authDto, ServerResponse res)
        {
            var account = await _repository.GetByUsernameAsync(authDto.UserName);

            if (account != null)
            {
                var accountDto = _mapper.Map<AccountDto>(account);
                var salt = account.Salt;
                var password = account.Password;
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(authDto.Password, salt);

                if (password == hashedPassword)
                {
                    var token = GenerateJwtToken(account);
                    accountDto.Token = token;
                    res.Data = accountDto;
                }
                else
                {
                    res.OnError
                    (
                        ErrorCode.AuthIncorrectPassword,
                        new ErrorItem("refPassword", _resource["AuthIncorrectPassword"])
                    );
                };
            }
            else
            {
                res.OnError
                (
                    ErrorCode.AuthNotExistsUsername,
                    new ErrorItem("refUsername", _resource["AuthNotExistsUsername"])
                );
            }
        }

        /// <summary>
        /// Tạo jwt token
        /// </summary>
        /// <param name="account">Tài khoản</param>
        /// <returns>Jwt token</returns>
        private string GenerateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (
                    new Claim[]
                    {
                        new Claim(Keys.ClaimAccountId, account.AccountId.ToString()),
                        new Claim(Keys.ClaimUsername, account.Username),
                        new Claim(Keys.ClaimRole, account.Role.ToString() ?? string.Empty),
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(_jwtConfig.ExpireToken),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Quên mật khẩu
        public async Task ForgotPasswordAsync(AuthDto authDto, ServerResponse res)
        {
            var account = await _repository.GetByUsernameAsync(authDto.UserName);

            if (account != null)
            {
                res.Data = new 
                {
                    account.AccountId,
                    account.Email
                };
            }
            else
            {
                res.OnError
                (
                    ErrorCode.AuthNotExistsUsername,
                    new ErrorItem("refUsername", _resource["AuthNotExistsUsername"])
                );
            }
        }

        // Lấy AccountId
        public Guid GetAccountId()
        {
            //var accountId = "1bf8a43c-47fb-4d6c-1863-eeb1d8ed8cef";
            var claim = GetClaim(Keys.ClaimAccountId);
            var accountId = claim != null && claim.Value != null ? Guid.Parse(claim.Value) : Guid.Empty;
            return accountId;
        }

        // Lấy RoleCode
        public RoleCode? GetRole()
        {
            try {
                var claim = GetClaim(Keys.ClaimRole);
                return claim != null && claim.Value != null ? (RoleCode)Convert.ToInt32(claim.Value) : null;
            }
            catch {
                return null;
            }
        }

        public string GetAccountIdToString()
        {
            return GetAccountId().ToString();
        }

        // AccessToken
        public string? GetToken()
        {
            var token = _httpContext.HttpContext.Request.Headers[Keys.Authorization].FirstOrDefault();
            return token;
        }

        // Get claims
        public Claim? GetClaim(string keyClaim)
        {
            try
            {
                var header = _httpContext.HttpContext.Request.Headers[Keys.Authorization].FirstOrDefault();
                header = header?[Keys.Bearer.Length..].Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                if (tokenHandler.ReadToken(header) is JwtSecurityToken decodedToken)
                {
                    var claim = decodedToken.Claims.FirstOrDefault(c => c.Type == keyClaim);
                    return claim;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}