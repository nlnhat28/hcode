
namespace HCode.Domain
{
    /// <summary>
    /// Contest account state
    /// </summary>
    public enum ContestAccountState
    {
        /// <summary>
        /// Đã rời đi
        /// </summary>
        Leave = 0,
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
