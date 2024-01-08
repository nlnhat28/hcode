namespace HCode.Domain
{
    /// <summary>
    /// Đếm số lượng theo kho
    /// </summary>
    /// Created by: nlnhat (08/09/2023)
    public class CountByWarehouseModel
    {
        /// <summary>
        /// Tên kho
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public int Count { get; set; }
    }
}
