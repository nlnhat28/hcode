
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
        #endregion
    }
}
