
namespace HCode.Domain
{
    /// <summary>
    /// Lớp bài toán
    /// </summary>
    public class ProblemTopic : Problem
    {
        #region Properties
        /// <summary>
        /// Topic
        /// </summary>
        public List<Topic>? Topics { get; set; }
        #endregion
    }
}
