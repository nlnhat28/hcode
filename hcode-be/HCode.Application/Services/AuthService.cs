using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Crypto.Generators;
namespace HCode.Application
{
    /// <summary>
    /// Triển khai service đơn vị tính từ giao diện
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class AuthService : BaseService<AuthDto, Account>, IAuthService
    {
        #region Fields
        /// <summary>
        /// Service gửi email
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// Repo auth
        /// </summary>
        private new readonly IAccountRepository _repository;
        /// <summary>
        /// Repo vai trò
        /// </summary>
        private new readonly IRoleRepository _roleRepository;
        /// <summary>
        /// Cache
        /// </summary>
        /// Created by: nlnhat (13/07/2023
        protected readonly IMemoryCache _cache;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service đơn vị tính
        /// </summary>
        /// <param name="repository">Repository đơn vị tính</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map đối tượng</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public AuthService(IAccountRepository repository, IRoleRepository roleRepository,
                           IStringLocalizer<Resource> resource, IMapper mapper, 
                           IUnitOfWork unitOfWork, IEmailService emailService, IMemoryCache cache)
                         : base(repository, resource, mapper, unitOfWork)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _emailService = emailService;
            _cache = cache;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validate tài khoản tạo mới
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
                res.OnError(
                    ErrorCode.AuthExistedUsername,
                    _resource["AuthExistedUsername"],
                    new ErrorData("refUsername", authDto.UserName, ErrorKey.FormItem)
                );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns>HashedPassword: Password đã hashed; Salt: Salt</returns>
        private (string hashedPassword, string salt) HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return (hashedPassword, salt);
        }
        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="authDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public async Task SignupAsync(AuthDto authDto, ServerResponse res)
        {
            await ValidateCreateAccountAsync(authDto, res);

            if (res.Success)
            {
                var role = await _roleRepository.GetByCodeAsync(RoleConstant.RoleCode.Admin);
                var roleId = (role != null) ? role.RoleId : Guid.Empty;

                var (password, salt) = HashPassword(authDto.Password);

                var accountId = Guid.NewGuid();

                var account = new Account()
                {
                    AccountId = accountId,
                    Username = authDto.UserName,
                    Password = password,
                    Email = authDto.Email ?? string.Empty,
                    Salt = salt,
                    RoleId = roleId,
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
        /// <summary>
        /// Gửi mã xác thực
        /// </summary>
        /// <returns></returns> 
        public async Task<string?> SendVerifyCodeAsync(AuthDto authDto, ServerResponse res)
        {
            if (!string.IsNullOrWhiteSpace(authDto.Email))
            {
                var random = new Random();
                var randomNumber = random.Next(0, 1000000);
                var verifyCode = randomNumber.ToString("D6");
                var expireTime = AuthConstant.VerifyTime;

                var message = new EmailMessage()
                {
                    To = authDto.Email,
                    Subject = _resource["AuthSubjectVerifyEmail"],
                    Content = string.Format(
                        _resource["AuthContentVerifyEmail"],
                        $"<b>{authDto.UserName}</b>",
                        $"<b style=\"color: #00f\">{verifyCode}</b>",
                        $"<b>{expireTime}</b>")
                };

                var sendResult = await _emailService.SendEmailAsync(message);

                res.Success = sendResult.Success;
                res.DevMsg = sendResult.DevMsg;

                if (!res.Success)
                {
                    res.OnError(ErrorCode.AuthVerifyError, _resource["AuthVerifyError"]);
                }
                else
                {
                    var options = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(AuthConstant.VerifyTime)
                    };

                    _cache.Set($"Username_{authDto.UserName}", verifyCode, options);
                }

                return verifyCode;
            }
            else
            {
                res.OnError(ErrorCode.AuthVerifyError, _resource["AuthVerifyError"]);
                return null;
            }
        }
        /// <summary>
        /// Xác thực tài khoản
        /// </summary>
        /// <returns></returns> 
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
                    res.OnError(
                        ErrorCode.AuthIncorrectVerifyCode,
                        _resource["AuthIncorrectVerifyCode"],
                        new ErrorData("refVerifyCode", authDto.VerifyCode, ErrorKey.FormItem)
                    );
                }
            }
            else
            {
                res.OnError(
                    ErrorCode.AuthIncorrectVerifyCode,
                    _resource["AuthIncorrectVerifyCode"],
                    new ErrorData("refVerifyCode", authDto.VerifyCode, ErrorKey.FormItem)
                );
            }
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="authDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public Task LoginAsync(AuthDto authDto, ServerResponse res)
        {
            throw new NotImplementedException();
        }

        public override Account MapCreateDtoToEntity(AuthDto entityDto)
        {
            throw new NotImplementedException();
        }

        public override Account MapUpdateDtoToEntity(AuthDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task ValidateAsync(AuthDto entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}