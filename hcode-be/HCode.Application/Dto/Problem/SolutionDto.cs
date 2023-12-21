
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Lời giải
    /// </summary>
    public class SolutionDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid SolutionId { get; set; }
        /// <summary>
        /// Id bài toán
        /// </summary>
        public Guid ProblemId { get; set; }
        /// <summary>
        /// Source code
        /// </summary>
        public string SourceCode { get; set; }
        /// <summary>
        /// Ngôn ngữ lập trình
        /// </summary>
        public Language Language { get; set; }
    }
}
