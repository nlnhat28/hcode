
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Param submit contest
    /// </summary>
    public class ContestSubmitDto
    {
        #region Properties
        /// <summary>
        /// Bài thi bài toán
        /// </summary>
        public Guid ContestProblemId { get; set; }
        /// <summary>
        /// Lời giải
        /// </summary>
        public ProblemDto ProblemDto { get; set; }
        #endregion
    }
}