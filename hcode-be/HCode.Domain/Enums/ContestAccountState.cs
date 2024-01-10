
namespace HCode.Domain
{
    /// <summary>
    /// Contest account state
    /// </summary>
    public enum ContestAccountState
    {
        /// <summary>
        /// Chưa thi
        /// </summary>
        Pending = 1,
        /// <summary>
        /// Đang thi
        /// </summary>
        Doing = 2,
        /// <summary>
        /// Đã thi
        /// </summary>
        Finish = 3,
    }
}
