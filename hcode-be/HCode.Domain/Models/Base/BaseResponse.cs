
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
        /// Mã lỗi nếu có
        /// </summary>
        public ErrorCode? ErrorCode { get; set; }
        /// <summary>
        /// Mã thành công nếu có
        /// </summary>
        public SuccessCode? SuccessCode { get; set; }
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
        /// Hàm tạo BaseResponse từ SuccessCode
        /// </summary>
        /// <param name="successCode">Message</param>
        public BaseResponse(SuccessCode successCode, string? devMsg = null)
        {
            Success = true;
            SuccessCode = successCode;
            DevMsg = devMsg ?? successCode.ToString();
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
                exception.Message,
                exception.Source,
                exception.Data,
                exception.StackTrace,
                TargetSite = exception.TargetSite?.ToString(),
            };
        }
        #endregion

        #region Methods
        /// <summary>
        /// Kiểm tra success hay không
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return Success == true;
        }
        /// <summary>
        /// Thành công
        /// </summary>
        public void OnSuccess(SuccessCode? successCode = null)
        {
            Success = true;
            SuccessCode = successCode;
        }
        /// <summary>
        /// Success response
        /// </summary>
        /// <param name="devMsg"></param>
        /// <param name="data"></param>
        public void OnSuccess(string? devMsg = null, object? data = null)
        {
            Success = true;
            Data = data;
            DevMsg = devMsg;
        }
        /// <summary>
        /// Error response
        /// </summary>
        /// <param name="devMsg"></param>
        /// <param name="data"></param>
        public void OnError(string? devMsg = null, object? data = null)
        {
            Success = false;
            Data = data;
            DevMsg = devMsg;
        }
        #endregion

    }
}
