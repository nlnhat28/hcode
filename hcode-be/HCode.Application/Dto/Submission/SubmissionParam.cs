
using HCode.Domain;
using System.Reflection;
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
        [EncodeBase64]
        public string? source_code { get; set; }
        /// <summary>
        /// Id ngôn ngữ lập trình
        /// </summary>
        public int language_id { get; set; }
        /// <summary>
        /// Đầu vào
        /// </summary>
        [EncodeBase64]
        public string? stdin { get; set; }
        /// <summary>
        /// Expected output
        /// </summary>
        [EncodeBase64]
        public string? expected_output { get; set; }
        /// <summary>
        /// Giới hạn thời gian (second)
        /// </summary>
        public double? cpu_time_limit { get; set; }
        /// <summary>
        /// Giới hạn bộ nhớ (kilobyte)
        /// </summary>
        public double? memory_limit { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Encode base64 1 số properties
        /// </summary>
        public void Encode()
        {
            AppHelper.EncodeBase64(this);
        }
        /// <summary>
        /// Decode base64 1 số properties
        /// </summary>
        public void Decode()
        {
            AppHelper.DecodeBase64(this);
        }
        /// <summary>
        /// Convert to json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion
    }
}
