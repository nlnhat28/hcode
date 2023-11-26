

using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Lớp bài toán
    /// </summary>
    public class ProblemDto : BaseDto
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid ProblemId { get; set; }
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
        /// Tên độ khó
        /// </summary>
        public string? DifficultyName { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public ProblemState? State { get; set; }
        /// <summary>
        /// Giới hạn thời gian (millisecond)
        /// </summary>
        public int? LimitTime { get; set; }
        /// <summary>
        /// Giới hạn bộ nhớ (byte)
        /// </summary>
        public int? LimitMemory { get; set; }
        /// <summary>
        /// Gợi ý
        /// </summary>
        public string? Hint { get; set; }
        /// <summary>
        /// Source code giải
        /// </summary>
        public string? Solution { get; set; } 
        /// <summary>
        /// Id tài khoản tạo
        /// </summary>
        public Guid? AccountId { get; set; }
        /// <summary>
        /// Chủ đề
        /// </summary>
        public List<Topic>? Topics { get; set; }

        /// <summary>
        /// Đánh giá
        /// </summary>
        public decimal? Rate { get; set; }
        /// <summary>
        /// Lượt xem
        /// </summary>
        public int? SeenCount { get; set; }
        /// <summary>
        /// Lượt comment
        /// </summary>
        public int? CommentCount { get; set; }
        /// <summary>
        /// Điểm tương tác
        /// </summary>
        public decimal? ReactionScore { get; set; }
        /// <summary>
        /// Tên các chủ đề
        /// </summary>
        public string? TopicNames { get; set; }
        /// <summary>
        /// Hiển trị trạng thái new hay không
        /// </summary>
        public bool? IsNew { get; set; }
        /// <summary>
        /// Trạng thái của người dùng
        /// </summary>
        public ProblemAccountState? ProblemAccountState { get; set; }
        #endregion
    }
}