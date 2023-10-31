using System.Runtime.Serialization;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp biểu diễn thực thể nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public class Material : BaseAuditEntity
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid MaterialId { get; set; }
        /// <summary>
        /// Nã nguyên vật liệu
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng đơn vị tính
        /// </summary>
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng nhà kho
        /// </summary>
        public Guid? WarehouseId { get; set; }
        /// <summary>
        /// Mã kho
        /// </summary>
        public string? WarehouseCode { get; set; }
        /// <summary>
        /// Tên kho
        /// </summary>
        public string? WarehouseName { get; set; }
        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public int? ExpiryTime { get; set; }
        /// <summary>
        /// Đơn vị thời gian hết hạn
        /// </summary>
        public TimeUnit? TimeUnit { get; set; }
        /// <summary>
        /// Số lượng tồn tối thiểu
        /// </summary>
        public decimal? MinimumInventory { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string? Note { get; set; }
        /// <summary>
        /// Ngừng theo dõi hay không
        /// </summary>
        public bool? IsUnfollow { get; set; }
    }
}
