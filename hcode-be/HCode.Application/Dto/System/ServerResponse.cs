
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
        /// <summary>
        /// Loại lỗi
        /// </summary>
        public ErrorKey? ErrorKey { get; set; }
        /// <summary>
        /// Data khác
        /// </summary>
        public List<object>? MoreData { get; set; }
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
        public void OnSuccess(SuccessCode? successCode = null)
        {
            Success = true;
            SuccessCode = successCode;
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
        /// <param name="exception"></param>
        public void OnError(Exception exception)
        {
            Success = false;
            DevMsg = exception.Message;
            Data = ExceptionToData(exception);
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="userMsg"></param>
        public void OnError(ErrorCode errorCode, string? userMsg = null)
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
        /// <param name="data"></param>
        public void OnError(ErrorCode errorCode, string? userMsg = null, object? data = null)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = userMsg;
            Data = data;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="userMsg"></param>
        /// <param name="exception"></param>
        public void OnError(ErrorCode errorCode, string? userMsg = null, Exception? exception = null)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = userMsg;
            DevMsg = exception?.Message;

            if (exception != null)
            {
                Data = ExceptionToData(exception);
            }
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
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="userMsg"></param>
        /// <param name="errorItem"></param>
        public void OnError(ErrorCode errorCode, string? userMsg, ErrorItem? errorItem)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = userMsg;
            Data = errorItem;
            ErrorKey = Domain.ErrorKey.FormItem;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorItem"></param>
        public void OnError(ErrorCode errorCode, ErrorItem? errorItem)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = errorItem?.ErrorMessage;
            Data = errorItem;
            ErrorKey = Domain.ErrorKey.FormItem;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="userMsg"></param>
        /// <param name="errorItems"></param>
        public void OnError(ErrorCode errorCode, string? userMsg, List<ErrorItem>? errorItems)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = userMsg ?? errorItems?.FirstOrDefault()?.ErrorMessage;
            Data = errorItems;
            ErrorKey = Domain.ErrorKey.FormItem;
        }
        /// <summary>
        /// Lỗi
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorItems"></param>
        public void OnError(ErrorCode errorCode, List<ErrorItem>? errorItems)
        {
            Success = false;
            ErrorCode = errorCode;
            UserMsg = errorItems?.FirstOrDefault()?.ErrorMessage;
            Data = errorItems;
            ErrorKey = Domain.ErrorKey.FormItem;
        }
        /// <summary>
        /// Thêm data khác
        /// </summary>
        /// <param name="data"></param>
        public void AddData(object data)
        {
            if (data != null)
            {
                MoreData ??= new List<object>();

                if (data is BaseResponse || data is ServerResponse)
                {
                    MoreData.Add(data);
                }
                else if (data is Exception exception)
                {
                    MoreData.Add(new BaseResponse(exception));
                }
                else if (data is string msg)
                {
                    MoreData.Add(new BaseResponse(devMsg: msg));
                }
                else
                {
                    MoreData.Add(new BaseResponse(data: data));
                }
            }
        }
        /// <summary>
        /// Thanh lọc exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static object ExceptionToData(Exception exception)
        {
            var data = new
            {
                exception.Message,
                exception.Source,
                exception.Data,
                exception.StackTrace,
                TargetSite = exception.TargetSite?.ToString(),
            };

            return data;
        }
        #endregion
    }
}
