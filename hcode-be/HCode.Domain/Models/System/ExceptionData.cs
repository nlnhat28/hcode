namespace HCode.Domain
{
    /// <summary>
    /// Data đính kèm exception
    /// </summary>
    /// Created by: nlnhat (18/07/2023)
    public class ExceptionData
    {
        #region Properties
        /// <summary>
        /// Loại exception data
        /// </summary>
        public ExceptionKey? ExceptionKey { get; set; }
        /// <summary>
        /// Mô tả loại exception data
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
        public ExceptionData(string? key = null, string? value = null, ExceptionKey? exceptionKey = null, string? description = null)
        {
            Key = key;
            Value = value;
            Description = description;
            ExceptionKey = exceptionKey;
        }
        #endregion
    }
}
