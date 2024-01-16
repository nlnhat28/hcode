
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Lớp bài thi bài toán
    /// </summary>
    public class ContestProblemDto : BaseDto, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid ContestProblemId { get; set; }
        public Guid Id
        {
            get { return ContestProblemId; }
            set { ContestProblemId = value; }
        }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        public Guid ContestId { get; set; }
        /// <summary>
        /// ID bài toán
        /// </summary>
        public Guid ProblemId { get; set; }
        /// <summary>
        /// Điểm
        /// </summary>
        public int? Score { get; set; }
        /// <summary>
        /// Thứ tự
        /// </summary>
        public int? Order { get; set; }
        #endregion
    }
}