
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài toán tài khoản
    /// </summary>
    [Table("problem_account")]
    public class ProblemAccount : BaseEntity, IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public Guid ProblemAccountId { get; set; }
        [NotMapped]
        public Guid Id
        {
            get { return ProblemAccountId; }
            set { ProblemAccountId = value; }
        }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        [Script(isNotUpdate: true)]
        public Guid ProblemId { get; set; }
        /// <summary>
        /// Id tài khoản
        /// </summary>
        [Script(isNotUpdate: true)]
        public Guid AccountId { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public ProblemAccountState State { get; set; }
        #endregion

        #region Constructors
        public ProblemAccount()
        {
        }
        public ProblemAccount(Guid problemId, Guid accountId, ProblemAccountState state) 
        {
            ProblemAccountId = Guid.NewGuid();
            ProblemId = problemId;
            AccountId = accountId;
            State = state;
        }
        #endregion
    }
}