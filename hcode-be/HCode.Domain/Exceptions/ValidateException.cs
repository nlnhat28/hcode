namespace HCode.Domain
{
    /// <summary>
    /// Lớp ngoại lệ khi validate dữ liệu
    /// </summary>
    /// Created by: nlnhat (15/07/2023)
    public class ValidateException : BaseException
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo ngoại lệ dữ liệu không hợp lệ
        /// </summary>
        /// <param name="errorCode">Mã lỗi nội bộ</param>
        /// <param name="userMsg">Thông báo đến người dùng</param>
        /// <param name="data">Dữ liệu di kèm</param>
        /// Created by: nlnhat (15/07/2023)
        public ValidateException(MISAErrorCode? errorCode = null, string? userMsg = null, object? data = null) : base(userMsg, data: data)
        {
            ErrorCode = errorCode ?? MISAErrorCode.InvalidData;
        }
        #endregion
    }
}
