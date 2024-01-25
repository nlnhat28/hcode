
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Lớp bài thi
    /// </summary>
    public class ContestDto : BaseDto, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid ContestId { get; set; }
        public Guid Id
        {
            get { return ContestId; }
            set { ContestId = value; }
        }
        /// <summary>
        /// Tên bài thi
        /// </summary>
        public string ContestName { get; set; }
        /// <summary>
        /// Mã bài thi
        /// </summary>
        public string? ContestCode { get; set; }
        /// <summary>
        /// Alias
        /// </summary>
        public string? Alias { get; set; }
        /// <summary>
        /// Độ khó
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public ContestState? State { get; set; }
        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// Thời gian làm bài (phút)
        /// </summary>
        public int? TimeToDo { get; set; }
        /// <summary>
        /// Công khai
        /// </summary>
        public bool IsPublic { get; set; } = false;
        /// <summary>
        /// Id tài khoản tạo
        /// </summary>
        public Guid? AccountId { get; set; }
        /// <summary>
        /// Trạng thái người dùng với bài thi
        /// </summary>
        public ContestAccountState? ContestAccountState { get; set; }
        /// <summary>
        /// Số lượng tham gia
        /// </summary>
        public int? JoinCount { get; set; }
        /// <summary>
        /// Danh sách câu hỏi
        /// </summary>
        [AutoMapper.Configuration.Annotations.Ignore]
        public List<ContestProblemDto>? ContestProblems { get; set; }
        #endregion
    }
}