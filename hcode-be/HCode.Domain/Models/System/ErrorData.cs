namespace HCode.Domain
{
    /// <summary>
    /// Data đính kèm response lỗi
    /// </summary>
    /// Created by: nlnhat (18/07/2023)
    public class ErrorData
    {
        #region Properties
        /// <summary>
        /// Loại lỗi
        /// </summary>
        public ErrorKey? ErrorKey { get; set; }
        /// <summary>
        /// Mô tả loại lỗi
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Từ khoá mô tả dữ liệu
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// Giá trị đính kèm
        /// </summary>
        /// <returns></returns>
        public string? Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo data ngoại lệ
        /// </summary>
        /// <param name="key">Từ khoá tìm kiếm dữ liệu</param>
        /// <param name="value">Giá trị</param>
        public ErrorData(string? key = null, string? value = null, ErrorKey? errorKey = null, string? description = null)
        {
            Key = key;
            Value = value;
            Description = description;
            ErrorKey = errorKey;
        }
        #endregion
    }
}
