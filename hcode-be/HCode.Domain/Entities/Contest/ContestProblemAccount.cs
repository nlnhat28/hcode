
namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài thi bài toán
    /// </summary>
    public class ContestProblemAccount : BaseEntity, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid ContestProblemAccountId { get; set; }
        public Guid Id
        {
            get { return ContestProblemAccountId; }
            set { ContestProblemAccountId = value; }
        }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        public Guid ContestProblemId { get; set; }
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public Guid AccountId { get; set; }
        #endregion
    }
}