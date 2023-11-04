using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Dto tạo tài khoản
    /// </summary>
    /// Created by: nlnhat (11/07/2023)
    public class AuthDto
    {
        #region Properties
        /// <summary>
        /// Tên tài khoản
        /// </summary>
        [MaxLength(50)]
        public string? UserName { get; set; }
        /// <summary>
        /// Mật khẩu (hashed)
        /// </summary>
        [MaxLength(255)]
        public string? Password { get; set; }
        /// <summary>
        /// Salt
        /// </summary>
        [MaxLength(255)]
        public string? Salt { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [MaxLength(100)]
        public string? Email { get; set; }
        /// <summary>
        /// Mã xác thực
        /// </summary>
        [MaxLength(6)]
        public string? VerifyCode { get; set; }
        #endregion
    }
}