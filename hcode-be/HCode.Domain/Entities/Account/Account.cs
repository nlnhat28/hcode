using System.ComponentModel.DataAnnotations;

namespace HCode.Domain
{
    /// <summary>
    /// Tài khoản
    /// </summary>
    public class Account : BaseEntity, IHasEntityId
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
        public string Username { get; set; }
        /// <summary>
        /// Mật khẩu đã hash
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public byte[]? Avatar { get; set; }
        /// <summary>
        /// Họ tên đầy đủ
        /// </summary>
        public string FullName { get; set; }
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
        public string? Salt { get; set; }
        /// <summary>
        /// Mã vai trò
        /// </summary>
        public RoleCode? Role { get; set; }
    }
}
