namespace HCode.Domain
{
    /// <summary>
    /// Lớp ngoại lệ khi không hoàn thành tác vụ
    /// </summary>
    /// Created by: nlnhat (15/07/2023)
    public class IncompleteException : BaseException
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo ngoại lệ không hoàn thành tác vụ
        /// </summary>
        /// <param name="errorCode">Mã lỗi nội bộ</param>
        /// <param name="userMsg">Thông báo đến người dùng</param>
        /// <param name="data">Dữ liệu di kèm</param>
        /// Created by: nlnhat (24/07/2023)
        public IncompleteException(MISAErrorCode? errorCode = null, string? userMsg = null, string? devMsg = null, object? data = null) 
             : base(userMsg, devMsg, data)
        {
            ErrorCode = errorCode ?? MISAErrorCode.IncompleteTask;
        }
        #endregion
    }
}
