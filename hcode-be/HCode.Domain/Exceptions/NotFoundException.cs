namespace HCode.Domain
{
    /// <summary>
    /// Lớp ngoại lệ không tìm thấy
    /// </summary>
    /// Created by: nlnhat (15/07/2023)
    public class NotFoundException : BaseException
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo ngoại lệ không tìm thấy dữ liệu
        /// <param name="errorCode">Mã lỗi nội bộ</param>
        /// <param name="userMsg">Thông báo đến người dùng</param>
        /// <param name="data">Dữ liệu di kèm</param>
        /// </summary>
        /// Created by: nlnhat (15/07/2023)
        public NotFoundException(MISAErrorCode? errorCode = null, string? userMsg = null, object? data = null) : base(userMsg, data: data)
        {
            ErrorCode = errorCode ?? MISAErrorCode.NotFound;
        }
        #endregion
    }
}
