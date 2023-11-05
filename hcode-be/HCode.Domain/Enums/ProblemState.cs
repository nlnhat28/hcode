
namespace HCode.Domain
{
    /// <summary>
    /// Trạng thái 1 bài toán
    /// </summary>
    public enum ProblemState
    {
        /// <summary>
        /// Nháp
        /// </summary>
        Draft = 1,
        /// <summary>
        /// Riêng tư
        /// </summary>
        Private = 2,
        /// <summary>
        /// Công khai như không kích hoạt
        /// </summary>
        DeactivatedPublic = 3,
        /// <summary>
        /// Công khai
        /// </summary>
        Public = 4,
    }
}
