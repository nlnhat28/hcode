
using System.ComponentModel.DataAnnotations;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài thi
    /// </summary>
    [CanSearch("ContestCode, ContestName")]
    public class Contest : BaseEntity, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public Guid ContestId { get; set; }
        [NotMapped]
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
        public string ContestCode { get; set; }
        /// <summary>
        /// Alias
        /// </summary>
        public string? Alias { get; set; }
        /// <summary>
        /// Độ khó
        /// </summary>
        public string? Password { get; set; }
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
        [Script(isNotUpdate: true)]
        public Guid? AccountId { get; set; }
        /// <summary>
        /// Danh sách problem
        /// </summary>
        [NotMapped]
        public List<ContestProblem>? ContestProblems { get; set; }
        /// <summary>
        /// Độ khó
        /// </summary>
        [NotMapped]
        public bool HasPassword { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        [NotMapped]
        public ContestState? State { get; set; }
        /// <summary>
        /// Trạng thái người dùng với bài thi
        /// </summary>
        [NotMapped]
        public ContestAccountState? ContestAccountState { get; set; }
        /// <summary>
        /// Số lượng tham gia
        /// </summary>
        [NotMapped]
        public int? JoinCount { get; set; }
        #endregion
    }
}