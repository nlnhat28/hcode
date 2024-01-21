
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài toán
    /// </summary>
    [CanSearch("ProblemCode, ProblemName, Topic")]
    public class Problem : BaseEntity, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public Guid ProblemId { get; set; }
        [NotMapped]
        public Guid Id
        {
            get { return ProblemId; }
            set { ProblemId = value; }
        }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        public string ProblemName { get; set; }
        /// <summary>
        /// Mã bài toán
        /// </summary>
        [NoUpdate]
        public int ProblemCode { get; set; }
        /// <summary>
        /// Alias
        /// </summary>
        public string? Alias { get; set; }
        /// <summary>
        /// Độ khó
        /// </summary>
        public Difficulty? Difficulty { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        [NoUpdate]
        public ProblemState? State { get; set; }
        /// <summary>
        /// Giới hạn thời gian (giây)
        /// </summary>
        public double? LimitTime { get; set; }
        /// <summary>
        /// Giới hạn bộ nhớ (kb)
        /// </summary>
        public double? LimitMemory { get; set; }
        /// <summary>
        /// Gợi ý
        /// </summary>
        public string? Hint { get; set; }
        /// <summary>
        /// Source code giải
        /// </summary>
        public string? Solution { get; set; }
        /// <summary>
        /// Id language giải
        /// </summary>
        public Guid? SolutionLanguageId { get; set; }
        /// <summary>
        /// Tên language giải
        /// </summary>
        [NotMapped]
        public string? LanguageName { get; set; }
        /// <summary>
        /// Id language giải
        /// </summary>
        [NotMapped]
        public int? JudgeId { get; set; }
        /// <summary>
        /// Id tài khoản tạo
        /// </summary>
        [NoUpdate]
        public Guid? AccountId { get; set; }
        /// <summary>
        /// Chủ đề
        /// </summary>
        public string? Topic { get; set; }
        /// <summary>
        /// Kiểu trả về
        /// </summary>
        public DataType? OutputType { get; set; }
        /// <summary>
        /// Nháp
        /// </summary>
        public bool? IsDraft { get; set; }
        /// <summary>
        /// Danh sách testcases
        /// </summary>
        [NotMapped]
        public List<Testcase>? Testcases { get; set; }
        /// <summary>
        /// Danh sách param
        /// </summary>
        [NotMapped]
        public List<Parameter>? Parameters{ get; set; }
        /// <summary>
        /// Đánh giá
        /// </summary>
        [NotMapped]
        public decimal? Rate { get; set; }
        /// <summary>
        /// Lượt xem
        /// </summary>
        [NotMapped]
        public int? SeenCount { get; set; }
        /// <summary>
        /// Lượt comment
        /// </summary>
        [NotMapped]
        public int? CommentCount { get; set; }
        /// <summary>
        /// Tỉ lệ accepted
        /// </summary>
        [NotMapped]
        public decimal? AcceptedRate { get; set; }
        /// <summary>
        /// Điểm tương tác
        /// </summary>
        [NotMapped]
        public decimal? ReactionScore { get; set; }
        /// <summary>
        /// Trạng thái của người dùng
        /// </summary>
        [NotMapped]
        public ProblemAccountState? ProblemAccountState { get; set; }
        #endregion
    }
}