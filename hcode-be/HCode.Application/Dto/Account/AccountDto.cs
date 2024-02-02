using HCode.Domain;
using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Tài khoản
    /// </summary>
    public class AccountDto : BaseDto, IHasEntityId
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid AccountId { get; set; }
        public Guid Id
        {
            get { return AccountId; }
            set { AccountId = value; }
        }
        /// <summary>
        /// Tên tài khoản
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        /// <summary>
        /// Họ tên đầy đủ
        /// </summary>
        [MaxLength(100)]
        public string? FullName { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MaxLength(50)]
        public string? Phone { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string? Avatar { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Đã xác thực email chưa
        /// </summary>
        public bool? IsVerified { get; set; }
        /// <summary>
        /// Salt hash password
        /// </summary>
        [MaxLength(255)]
        public string? Salt { get; set; }
        /// <summary>
        /// Auth token
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// Mã vai trò
        /// </summary>
        public RoleCode? Role { get; set; }
    }
}
