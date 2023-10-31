using Newtonsoft.Json;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp exception nội bộ
    /// </summary>
    /// Created by: nlnhat (15/07/2023)
    public class MISAException
    {
        #region Properties
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public MISAErrorCode? ErrorCode { get; set; }
        /// <summary>
        /// Thông báo cho Dev
        /// </summary>
        public string DevMsg { get; set; }
        /// <summary>
        /// Thông báo cho người dùng
        /// </summary>
        public string UserMsg { get; set; }
        /// <summary>
        /// Mã tra cứu thông tin lỗi
        /// </summary>
        public string? TraceId { get; set; }
        /// <summary>
        /// Thông tin thêm về lỗi
        /// </summary>
        public string? MoreInfo { get; set; }
        /// <summary>
        /// Dữ liệu đính kèm
        /// </summary>
        public object? Data { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Chuyển đối tượng sang định dạng json
        /// </summary>
        /// <returns>Chuỗi json</returns>
        /// Created by: nlnhat (18/07/2023)
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}