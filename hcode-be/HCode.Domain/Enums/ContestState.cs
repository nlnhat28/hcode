
namespace HCode.Domain
{
    /// <summary>
    /// Contest state
    /// </summary>
    public enum ContestState
    {
        /// <summary>
        /// Chưa mở
        /// </summary>
        Pending = 1,
        /// <summary>
        /// Đang diễn ra
        /// </summary>
        GoingOn = 2,
        /// <summary>
        /// Đã kết thúc
        /// </summary>
        Finish = 3,
    }
}
