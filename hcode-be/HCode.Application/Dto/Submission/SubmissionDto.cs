

using AutoMapper.Configuration.Annotations;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Nộp code
    /// </summary>
    public class SubmissionDto : BaseDto, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid SubmissionId { get; set; }
        public Guid Id
        {
            get { return SubmissionId; }
            set { SubmissionId = value; }
        }
        /// <summary>
        /// Id cha
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// Sourcecode
        /// </summary>
        public string? SourceCode { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public Guid? LanguageId { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public string? LanguageName { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public StatusJudge0? StatusId { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// Thời gian chạy
        /// </summary>
        public double? RunTime { get; set; }
        /// <summary>
        /// Bộ nhớ chạy
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
        /// Thời gian tạo
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        #endregion
    }
}