
namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài toán
    /// </summary>
    public class Problem : BaseAuditEntity
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid Problemld { get; set; }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        public string ProblemName { get; set; }
        /// <summary>
        /// Mã bài toán
        /// </summary>
        public string ProblemCode { get; set; }
        /// <summary>
        /// Alias
        /// </summary>
        public string? Alias { get; set; }
        /// <summary>
        /// Số (1 kiểu mã)
        /// </summary>
        public int? Number { get; set; }
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
        /// Source code giảis
        /// </summary>
        public string? Solution { get; set; } 
        #endregion
    }
}
