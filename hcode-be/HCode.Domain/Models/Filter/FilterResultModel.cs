namespace HCode.Domain
{
    /// <summary>
    /// Mô hình kết quả lọc đối tượng
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public class FilterResultModel<TEntity>
    {
        #region Properties
        /// <summary>
        /// Tổng số bản ghi được lọc
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// Tổng số bản ghi trong database
        /// </summary>
        public int AllRecord { get; set; }
        /// <summary>
        /// Thông tin phân trang
        /// </summary>
        public PagingModel? PagingModel { get; set; }
        /// <summary>
        /// Danh sách đối tượng thoả mãn điều kiện lọc
        /// </summary>
        public IEnumerable<TEntity>? Data { get; set; }
        #endregion
    }
}