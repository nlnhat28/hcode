namespace HCode.Domain
{
    /// <summary>
    /// Items lỗi
    /// </summary>
    /// Created by: nlnhat (18/07/2023)
    public class ErrorItem
    {
        #region Properties
        /// <summary>
        /// Tên ref lỗi
        /// </summary>
        public string? RefName { get; set; }
        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string? ErrorMessage { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="refName">Tên ref lỗi</param>
        /// <param name="errorMessage">Thông báo lỗi</param>
        public ErrorItem(string? refName = null, string? errorMessage = null)
        {
            RefName = refName;
            ErrorMessage = errorMessage;
        }
        #endregion
    }
}
