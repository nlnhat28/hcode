
using System.Text.Json;

namespace HCode.Application
{
    /// <summary>
    /// Submission gửi đến server thực thi code
    /// </summary>
    public class SubmissionParam
    {
        #region Properties
        /// <summary>
        /// Mã nguồn (base64 encode)
        /// </summary>
        public string? source_code { get; set; }
        /// <summary>
        /// Id ngôn ngữ lập trình
        /// </summary>
        public int language_id { get; set; }
        /// <summary>
        /// Đầu vào
        /// </summary>
        public string? stdin { get; set; }
        /// <summary>
        /// Expected output
        /// </summary>
        public string? expected_output { get; set; }
        /// <summary>
        /// Giới hạn thời gian (second)
        /// </summary>
        public decimal? cpu_time_limit { get; set; }
        /// <summary>
        /// Giới hạn bộ nhớ (kilobyte)
        /// </summary>
        public decimal? memory_limit { get; set; }
        #endregion

        #region Methods
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion
    }
}
