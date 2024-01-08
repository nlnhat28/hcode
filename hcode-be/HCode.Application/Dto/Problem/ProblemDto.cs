

using AutoMapper.Configuration.Annotations;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Lớp bài toán
    /// </summary>
    public class ProblemDto : BaseDto, IHasEntityId
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
        /// Giới hạn thời gian (giây)
        /// </summary>
        public decimal? LimitTime { get; set; }
        /// <summary>
        /// Đơn vị thời gian 
        /// </summary>
        public TimeUnit TimeUnit { get; set; }
        /// <summary>
        /// Giới hạn bộ nhớ (Kb)
        /// </summary>
        public decimal? LimitMemory { get; set; }
        /// <summary>
        /// Đơn vị bộ nhớ
        /// </summary>
        public MemoryUnit MemoryUnit { get; set; }
        /// <summary>
        /// Gợi ý
        /// </summary>
        public string? Hint { get; set; }
        /// <summary>
        /// Source code giải
        /// </summary>
        public string? Solution { get; set; }
        /// <summary>
        /// Source code giải
        /// </summary>
        public Language? SolutionLanguage { get; set; }
        /// <summary>
        /// Id tài khoản tạo
        /// </summary>
        public Guid? AccountId { get; set; }
        /// <summary>
        /// Chủ đề
        /// </summary>
        public string? Topic { get; set; }
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
        /// Hiển trị trạng thái new hay không
        /// </summary>
        public bool? IsNew { get; set; }
        /// <summary>
        /// Lưu thành công khai
        /// </summary>
        public bool? IsPublicState { get; set; }
        /// <summary>
        /// Lưu thành riêng tư
        /// </summary>
        public bool? IsPrivateState { get; set; }
        /// <summary>
        /// Kiểu trả về
        /// </summary>
        public DataType? OutputType { get; set; }
        /// <summary>
        /// Trạng thái của người dùng
        /// </summary>
        public ProblemAccountState? ProblemAccountState { get; set; }
        /// <summary>
        /// Danh sách testcases
        /// </summary>
        [AutoMapper.Configuration.Annotations.Ignore]
        public List<TestcaseDto>? Testcases { get; set; }
        /// <summary>
        /// Danh sách param
        /// </summary>
        [AutoMapper.Configuration.Annotations.Ignore] 
        public List<ParameterDto>? Parameters { get; set; }
        #endregion
    }
}