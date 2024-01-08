
namespace HCode.Domain
{
    /// <summary>
    /// Lớp vai trò
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// Mã vai trò
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// Tên vai trò
        /// </summary>
        public string? RoleName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Thứ tự
        /// </summary>
        public int SortOrder { get; set; }
        
    }
}
