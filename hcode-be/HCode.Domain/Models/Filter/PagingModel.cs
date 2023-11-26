namespace HCode.Domain
{
    /// <summary>
    /// Mô hình phân trang
    /// </summary>
    /// Created by: nlnhat (16/07/2023)
    public class PagingModel
    {
        #region Properties
        /// <summary>
        /// Trang muốn lấy số liệu
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Số bản ghi trên 1 trang
        /// </summary>
        public int PageSize { get; set; }
        #endregion
    }
}
