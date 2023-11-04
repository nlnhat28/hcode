using HCode.Application;
using HCode.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HCode.Api
{
    /// <summary>
    /// Controller Auth
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class AuthController : BaseController<AuthDto, Account>
    {
        #region Fields
        /// <summary>
        /// Service nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IAuthService _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        /// <param name="service">Service nguyên vật liệu</param>
        /// Created by: nlnhat (17/08/2023)
        public AuthController(IAuthService service, IWebHostEnvironment webHostEnvironment) : base(service, webHostEnvironment)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="authDto"></param>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("Signup")]
        public async Task<IActionResult> SignupAsync([FromBody] AuthDto authDto)
        {
            var res = new ServerResponse();
            await _service.SignupAsync(authDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Xác thực tài khoản
        /// </summary>
        /// <param name="authDto"></param>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("Verify")]
        public async Task<IActionResult> VerifyAsync([FromBody] AuthDto authDto)
        {
            var res = new ServerResponse();
            await _service.SignupAsync(authDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Gửi email xác thực
        /// </summary>
        /// <param name="authDto"></param>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("SendVerifyCode")]
        public async Task<IActionResult> SendVerifyCodeAsync([FromBody] AuthDto authDto)
        {
            var res = new ServerResponse();
            await _service.SendVerifyCodeAsync(authDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="authDto"></param>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthDto authDto)
        {
            var res = new ServerResponse();
            await _service.LoginAsync(authDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        #endregion
    }
}