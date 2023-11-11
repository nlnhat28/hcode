
namespace HCode.Domain
{
    /// <summary>
    /// Response cơ sở
    /// </summary>
    public class BaseResponse 
    {
        #region Fields
        /// <summary>
        /// Thành công hay không
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// Nhắn cho dev
        /// </summary>
        public string? DevMsg { get; set; }
        /// <summary>
        /// Data trả về
        /// </summary>
        public object? Data { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo BaseResponse full
        /// </summary>
        /// <param name="success">Thành công hay không</param>
        /// <param name="devMsg">Message</param>
        /// <param name="data">Data đính kèm</param>
        public BaseResponse(bool? success = true, string? devMsg = null, object? data = null)
        {
            Success = success ?? true;
            DevMsg = devMsg;
            Data = data;
        }
        /// <summary>
        /// Hàm tạo BaseResponse từ message
        /// </summary>
        /// <param name="devMsg">Message</param>
        public BaseResponse(string devMsg)
        {
            Success = true;
            DevMsg = devMsg;
        }
        /// <summary>
        /// Hàm tạo BaseResponse từ exception
        /// </summary>
        /// <param name="exception">Exception</param>
        public BaseResponse(Exception exception)
        {
            Success = false;
            DevMsg = exception.Message;
            Data = new
            {
                exception.Source,
                exception.Data,
                exception.StackTrace,
                TargetSite = exception.TargetSite?.ToString(),
            };
        }
        #endregion
    }
}
