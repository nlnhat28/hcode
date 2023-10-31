namespace HCode.Domain
{
    /// <summary>
    /// Lớp ngoại lệ cơ sở
    /// </summary>
    /// Created by: nlnhat (15/07/2023)
    public abstract class BaseException : Exception
    {
        #region Properties
        /// <summary>
        /// Mã lỗi nội bộ
        /// </summary>
        /// Created by: nlnhat (15/07/2023)
        public MISAErrorCode ErrorCode { get; set; }
        /// <summary>
        /// Thông báo lỗi cho dev
        /// </summary>
        /// Created by: nlnhat (15/07/2023)
        public string? DevMsg { get; set; }
        /// <summary>
        /// Thông báo lỗi cho người dùng
        /// </summary>
        /// Created by: nlnhat (15/07/2023)
        public string? UserMsg { get; set; }
        /// <summary>
        /// Dữ liệu thông báo lỗi (nếu có)
        /// </summary>
        /// Created by: nlnhat (15/07/2023)
        public object? Data { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo ngoại lệ cơ sở
        /// </summary>
        /// <param name="userMsg">Thông báo cho người dùng</param>
        /// <param name="data">Dữ liệu đính kèm</param>
        /// Created by: nlnhat (15/07/2023)
        public BaseException(string? userMsg = null, string? devMsg = null, object? data = null)
        {
            UserMsg = userMsg;
            DevMsg = devMsg;
            Data = data;
        }
        #endregion
    }
}
