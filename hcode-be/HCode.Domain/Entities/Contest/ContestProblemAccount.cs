
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài thi bài toán
    /// </summary>
    [Table("contest_problem_account")]
    public class ContestProblemAccount : BaseEntity, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public Guid ContestProblemAccountId { get; set; }
        [NotMapped]
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