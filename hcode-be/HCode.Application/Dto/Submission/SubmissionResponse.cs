
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
        /// Status
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// Thời gian trung bình
        /// </summary>
        public double? RunTime { get; set; }
        /// <summary>
        /// Bộ nhớ trung bình
        /// </summary>
        public double? Memory { get; set; }
        /// <summary>
        /// Số testcase qua
        /// </summary>
        public int? PassedCount { get; set; }
        /// <summary>
        /// Số testcase lỗi
        /// </summary>
        public int? FailedCount { get; set; }
        /// <summary>
        /// Tính toán thời gian, bộ nhớ
        /// </summary>
        public void CalculateTimeAndMemory()
        {
            RunTime = Submissions.Max(submit => Convert.ToDouble(submit.time, CultureInfo.InvariantCulture));
            Memory = Submissions.Max(submit => submit.memory);
        }
        /// <summary>
        /// Tính toán các kết quả
        /// </summary>
        public void CalculateResult()
        {
            PassedCount = 0;
            FailedCount = 0;

            if (Submissions.Count > 0)
            {
                var status = StatusJudge0.Accepted;

                foreach (var item in Submissions)
                {
                    if (item.status_id == StatusJudge0.Accepted)
                    {
                        PassedCount++;
                        continue;
                    }

                    FailedCount++;

                    if (item.status_id != StatusJudge0.OverLimit || status != StatusJudge0.WrongAnswer)
                    {
                        status = item.status_id;
                    }

                    if (item.status_id != StatusJudge0.OverLimit && item.status_id != StatusJudge0.WrongAnswer)
                    {
                        break;
                    }

                }

                StatusId = status;
                StatusName = status.ToString();

                if (status == StatusJudge0.Accepted || status == StatusJudge0.OverLimit || status == StatusJudge0.WrongAnswer)
                {
                    CalculateTimeAndMemory();
                }
            }
        }
    }
}
