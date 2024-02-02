
namespace HCode.Domain
{
    /// <summary>
    /// Trạng thái 1 bài toán
    /// </summary>
    public enum ProblemState
    {
        /// <summary>
        /// Công khai
        /// </summary>
        Public = 1,
        /// <summary>
        /// Công khai nhưng không kích hoạt
        /// </summary>
        DeactivatedPublic = 2,
        /// <summary>
        /// Riêng tư
        /// </summary>
        Private = 3,
        /// <summary>
        /// Nháp
        /// </summary>
        Draft = 4,
    }
}
