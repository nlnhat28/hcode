
using HCode.Domain;
using System.Text.Json;

namespace HCode.Application
{
    /// <summary>
    /// Response trả về client
    /// </summary>
    public class ServerResponse : BaseResponse
    {
        #region Properties
        /// <summary>
        /// Mã lỗi nếu có
        /// </summary>
        public ErrorCode? ErrorCode { get; set; }
        /// <summary>
        /// Nhắn cho người dùng
        /// </summary>
        public string? UserMsg { get; set; }
        /// <summary>
        /// Mã tra cứu thông tin lỗi
        /// </summary>
        public string? TraceId { get; set; }
        /// <summary>
        /// Thông tin thêm về lỗi
        /// </summary>
        public string? MoreInfo { get; set; } 
        #endregion

        #region Methods
        /// <summary>
        /// Chuyển đối tượng sang định dạng json
        /// </summary>
        /// <returns>Chuỗi json</returns>
        /// Created by: nlnhat (18/07/2023)
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        /// <summary>
        /// Thành công
        /// </summary>
        public void OnSuccess() 
        {
            Success = true;
        }
        /// <summary>
        /// Thành công
        /// </summary>
        /// <param name="data"></param>
        public void OnSuccess(object? data)
        {
            Success = true;
            Data = data;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        public void OnError()
        {
            Success = false;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="userMsg"></param>
        public void OnError(ErrorCode errorCode, string? userMsg)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = userMsg;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="userMsg"></param>
        /// <param name="errorData"></param>
        public void OnError(ErrorCode errorCode, string? userMsg, ErrorData? errorData)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = userMsg;
            Data = errorData;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="userMsg"></param>
        /// <param name="errorDatas"></param>
        public void OnError(ErrorCode errorCode, string? userMsg, List<ErrorData>? errorDatas)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = userMsg;
            Data = errorDatas;
        }
        #endregion
    }
}
