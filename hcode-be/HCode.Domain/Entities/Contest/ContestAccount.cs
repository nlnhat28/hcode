
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài thi tài khoản
    /// </summary>
    [Table("contest_account")]
    public class ContestAccount : BaseEntity, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public Guid ContestAccountId { get; set; }
        public Guid Id
        {
            get { return ContestAccountId; }
            set { ContestAccountId = value; }
        }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        [Script(isNotUpdate: true)]
        public Guid ContestId { get; set; }
        /// <summary>
        /// Id tài khoản
        /// </summary>
        [Script(isNotUpdate: true)]
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
        #endregion

        #region Constructors
        public ContestAccount(Guid contestId, Guid accountId, ContestAccountState? state = ContestAccountState.Pending, DateTime? 
            startTime = null, int? usedTime = 0) 
        {
            ContestAccountId = Guid.NewGuid();
            ContestId = contestId;
            AccountId = accountId;
            State = state;
            StartTime = startTime;
            UsedTime = usedTime;
        }
        #endregion

        #region 
        /// <summary>
        /// Chuyển trạng thái bắt đầu
        /// </summary>
        public void OnStart() 
        {
            StartTime = DateTime.UtcNow();
            State = ContestAccountState.Doing;
        }
        /// <summary>
        /// Chuyển trạng thái kết thúc
        /// </summary>
        public void OnFinish() 
        {
            var duration = DateTime.UtcNow() - contestAccount.StartTime;
            var usedTime = Convert.ToInt32(duration.TotalSeconds);
            State = ContestAccountState.Finish;
            UsedTime = usedTime;
        }
        #endregion
    }
}