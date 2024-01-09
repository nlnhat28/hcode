
namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài toán
    /// </summary>
    [CanSearch("ContestCode, ContestName")]
    public class Contest : BaseEntity, IHasEntityId
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
        /// Tên bài toán
        /// </summary>
        public string ContestName { get; set; }
        /// <summary>
        /// Mã bài toán
        /// </summary>
        public int ContestCode { get; set; }
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
        /// Công khai công
        /// </summary>
        public bool isPublic { get; set; } = false;
        #endregion
    }
}