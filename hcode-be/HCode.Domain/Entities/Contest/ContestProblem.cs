
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài thi bài toán
    /// </summary>
    [Table("contest_problem")]
    public class ContestProblem
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public Guid ContestProblemId { get; set; }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        [Script(isNotUpdate: true)]
        public Guid ContestId { get; set; }
        /// <summary>
        /// ID bài toán
        /// </summary>
        public Guid ProblemId { get; set; }
        /// <summary>
        /// Tên bài toán
        /// </summary>
        [NotMapped]
        public string? ProblemName { get; set; }
        /// <summary>
        /// Điểm
        /// </summary>
        public int? Score { get; set; }
        /// <summary>
        /// Thứ tự
        /// </summary>
        public int? Order { get; set; }
        /// <summary>
        /// Đúng hay sai
        /// </summary>
        [NotMapped]
        public ProblemAccountState? ContestProblemAccountState { get; set; }
        /// <summary>
        /// ContestProblemAccountId
        /// </summary>
        [NotMapped]
        public Guid ContestProblemAccountId { get; set; }
        #endregion
    }
}