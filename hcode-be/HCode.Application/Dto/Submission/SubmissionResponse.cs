
using AutoMapper.Configuration.Annotations;
using HCode.Domain;
using System.Globalization;
using System.Reflection;

namespace HCode.Application
{
    /// <summary>
    /// Response server thực thi code gửi về
    /// </summary>
    public class SubmissionResponse
    {
        #region Properties
        /// <summary>
        /// Trạng thái
        /// </summary>
        public StatusJudge0 status_id { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        [Domain.Ignore]
        public string? status_name { get; set; }
        /// <summary>
        /// Id testcase
        /// </summary>
        [Domain.Ignore]
        public Guid? testcase_id { get; set; }
        /// <summary>
        /// Thông báo cho người dùng
        /// </summary>
        [Domain.Ignore]
        public string? user_msg { get; set; }
        /// <summary>
        /// Id submission
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// Kết quả khi thực thi source code
        /// </summary>
        [EncodeBase64]
        public string stdout { get; set; }
        /// <summary>
        /// Lỗi
        /// </summary>
        [EncodeBase64]
        public string stderr { get; set; }
        /// <summary>
        /// Kết quả compile
        /// </summary>
        [EncodeBase64]
        public string compile_output { get; set; }
        /// <summary>
        /// Thông báo khác
        /// </summary>
        [EncodeBase64]
        public string message { get; set; }
        /// <summary>
        /// Exit code
        /// </summary>
        public int? exit_code { get; set; } = 0;
        /// <summary>
        /// Thời gian thực thi code (second)
        /// </summary>
        public string? time { get; set; } = "0";
        /// <summary>
        /// Bộ nhớ sử dụng (kilobyte)
        /// </summary>
        public double? memory { get; set; } = 0;
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
        /// Lấy tên status
        /// </summary>
        public void GenStatusName()
        {
            status_name = status_id.ToString();
        }
        #endregion
    }
    /// <summary>
    /// Danh sách submission response từ ceService
    /// </summary>
    public class Submissions
    {
        public List<SubmissionResponse> submissions { get; set; }
    }
}
