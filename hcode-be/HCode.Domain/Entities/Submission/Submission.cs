
namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài thi
    /// </summary>
    [CanSearch("ContestCode, ContestName")]
    public class Submission : BaseEntity, IHasEntityId
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
        public StatusJudge0? Status { get; set; }
        /// <summary>
        /// Thời gian chạy
        /// </summary>
        public decimal? RunTime { get; set; }
        /// <summary>
        /// Bộ nhớ chạy
        /// </summary>
        public decimal? Memory { get; set; }
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
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        [NotMapped]
        public Language? Language { get; set; }
        #endregion
    }
}