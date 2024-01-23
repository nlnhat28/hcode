
using System.ComponentModel.DataAnnotations;

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
        [Key]
        public Guid SubmissionId { get; set; }
        [NotMapped]
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
        /// <summary>
        /// Người tạo
        /// </summary>
        [NotMapped]
        [Script(isNotUpdate: true)]
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [NotMapped]
        public DateTime? ModifiedTime { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>  
        [NotMapped]
        public string? ModifiedBy { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        [NotMapped]
        public string? LanguageName { get; set; }
        #endregion

        #region Constructors
        #endregion
    }
}