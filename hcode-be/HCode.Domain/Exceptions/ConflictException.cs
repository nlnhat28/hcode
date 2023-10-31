namespace HCode.Domain
{
    /// <summary>
    /// Lớp ngoại lệ khi dữ liệu xung đột
    /// </summary>
    /// Created by: nlnhat (15/07/2023)
    public class ConflictException : BaseException
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo ngoại lệ xung đột dữ liệu
        /// </summary>
        /// <param name="errorCode">Mã lỗi nội bộ</param>
        /// <param name="userMsg">Thông báo đến người dùng</param>
        /// <param name="data">Dữ liệu di kèm</param>
        /// Created by: nlnhat (15/07/2023)
        public ConflictException(MISAErrorCode? errorCode = null, string? userMsg = null, object? data = null) : base(userMsg, data: data)
        {
            ErrorCode = errorCode ?? MISAErrorCode.ConflictData;
        }
        #endregion
    }
}
