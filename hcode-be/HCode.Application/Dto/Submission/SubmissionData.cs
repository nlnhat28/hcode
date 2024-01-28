
using AutoMapper.Configuration.Annotations;
using HCode.Domain;
using System.Globalization;
using System.Reflection;

namespace HCode.Application
{
    /// <summary>
    /// Submission data
    /// </summary>
    public class SubmissionData
    {
        #region Properties
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
        #endregion

        #region Methods
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
        /// <summary>
        /// Tạo mới submission từ submissionData
        /// </summary>
        /// <param name="data"></param>
        /// <param name="problemDto"></param>
        /// <returns></returns>
        public Submission InitSubmission(string sourceCode, Guid languageId, 
            Guid? problemAccountId = null, Guid? contestProblemAccountId = null)
        {
            var submission = new Submission()
            {
                SubmissionId = Guid.NewGuid(),
                PassedCount = PassedCount,
                FailedCount = FailedCount,
                StatusId = StatusId,
                StatusName = StatusName,
                RunTime = RunTime,
                Memory = Memory,
                CreatedTime = DateTime.UtcNow,
                ProblemAccountId = problemAccountId,
                ContestProblemAccountId = contestProblemAccountId,
                SourceCode = sourceCode,
                LanguageId = languageId,
            };
            return submission;
        }
        #endregion
    }
}
