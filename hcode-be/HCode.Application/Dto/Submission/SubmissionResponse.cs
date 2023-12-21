
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Response server thực thi code gửi về
    /// </summary>
    public class SubmissionResponse
    {
        /// <summary>
        /// Trạng thái
        /// </summary>
        public StatusJudge0 status_id { get; set; }
        /// <summary>
        /// Id submission
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// Kết quả khi thực thi source code
        /// </summary>
        public string stdout { get; set; }
        /// <summary>
        /// Lỗi
        /// </summary>
        public string stderr { get; set; }
        /// <summary>
        /// Kết quả compile
        /// </summary>
        public string compile_output { get; set; }
        /// <summary>
        /// Thông báo khác
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// Exit code
        /// </summary>
        public int exit_code { get; set; }
        /// <summary>
        /// Thời gian thực thi code (second)
        /// </summary>
        public decimal time { get; set; }
        /// <summary>
        /// Bộ nhớ sử dụng (kilobyte)
        /// </summary>
        public decimal memory { get; set; }
    }

    public class Submissions
    {
        public List<SubmissionResponse> submissions { get; set; }
    }
}
