
using AutoMapper.Configuration.Annotations;
using HCode.Domain;
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
        public decimal? memory { get; set; } = 0;
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
    /// <summary>
    /// Submission data
    /// </summary>
    public class SubmissionData
    {
        /// <summary>
        /// Danh sách submission response
        /// </summary>
        public List<SubmissionResponse> Submissions { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public StatusJudge0 StatusId { get; set; }
        /// <summary>
        /// Thời gian trung bình
        /// </summary>
        public decimal? AverageTime { get; set; }
        /// <summary>
        /// Bộ nhớ trung bình
        /// </summary>
        public decimal? AverageMemory { get; set; }
        /// <summary>
        /// Tính toán các giá trị trung bình
        /// </summary>
        public void CalculateAverage()
        {
            AverageTime = Submissions.Average(submit => Convert.ToDecimal(submit.time));
            AverageMemory = Submissions.Average(submit => submit.memory);
        }
    }
}
