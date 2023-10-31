namespace HCode.Domain
{
    /// <summary>
    /// Lớp thực thể nhà kho
    /// </summary>
    /// Created by: nlnhat (15/08/2023)
    public class Warehouse : BaseAuditEntity
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid WarehouseId { get; set; }
        /// <summary>
        /// Mã nhà kho
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Tên nhà kho
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }
    }
}
