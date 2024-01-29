
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Lớp bài thi tài khoản
    /// </summary>
    public class ContestAccountDto : BaseDto, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid ContestAccountId { get; set; }
        public Guid Id
        {
            get { return ContestAccountId; }
            set { ContestAccountId = value; }
        }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        public Guid ContestId { get; set; }
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public Guid AccountId { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public ContestAccountState? State { get; set; }
        /// <summary>
        /// Thời gian bắt đầu thi
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Thời gian sử dụng
        /// </summary>
        public int? UsedTime { get; set; }
        /// <summary>
        /// Tổng số điểm
        /// </summary>
        public int? TotalScore { get; set; }
        #endregion
    }
}