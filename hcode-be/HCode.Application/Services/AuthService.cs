using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using MimeKit.Cryptography;

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
        private readonly IAuthRepository _repository;
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
        public AuthService(IAuthRepository repository, IStringLocalizer<Resource> resource,
                           IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService, IMemoryCache cache)
                         : base(repository, resource, mapper, unitOfWork)
        {
            _repository = repository;
            _emailService = emailService;
            _cache = cache;
        }
        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="authDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public async Task SignupAsync(AuthDto authDto, ServerResponse res)
        {
            await SendVerifyCodeAsync(authDto, res);
        }
        /// <summary>
        /// Gửi mã xác thực
        /// </summary>
        /// <returns></returns> 
        public async Task SendVerifyCodeAsync(AuthDto authDto, ServerResponse res)
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

                res.Success = true;
                res.DevMsg = message.Content;

                if (!res.Success)
                {
                    res.UserMsg = _resource["AuthVerifyError"];
                }
            }
            else
            {
                res.Success = false;
                res.ErrorCode = ErrorCode.AuthVerify;
                res.UserMsg = _resource["AuthSignupError"];
            }
        }
        /// <summary>
        /// Xác thực tài khoản
        /// </summary>
        /// <returns></returns> 
        public async Task VerifyAsync(AuthDto authDto, ServerResponse res)
        {

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

        #region Methods

        #endregion
    }
}