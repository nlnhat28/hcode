namespace HCode.Domain
{
    /// <summary>
    /// Lớp thực thể đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (15/08/2023)
    public class ConversionUnit
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid ConversionUnitId { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng nguyên vật liệu
        /// </summary>
        public Guid MaterialId { get; set; }
        /// <summary>
        /// Tên đơn vị tính chính
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng đơn vị muốn chuyển
        /// </summary>
        public Guid DestinationUnitId { get; set; }
        /// <summary>
        /// Tên đơn vị muốn chuyển
        /// </summary>
        public string? DestinationUnitName { get; set; }
        /// <summary>
        /// Tỉ lệ chuyển đổi
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// Phép toán chuyển đổi
        /// </summary>
        public Operator Operator { get; set; }
        /// <summary>
        /// Thứ tự trong bảng trên giao diện
        /// </summary>
        public int RowIndex { get; set; }
    }
}
