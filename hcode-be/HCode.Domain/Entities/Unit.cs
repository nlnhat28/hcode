namespace HCode.Domain
{
    /// <summary>
    /// Lớp thực thể đơn vị tính
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public class Unit : BaseAuditEntity
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }
    }
}
