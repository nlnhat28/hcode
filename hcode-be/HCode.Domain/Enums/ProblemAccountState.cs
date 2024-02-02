

namespace HCode.Domain
{
    /// <summary>
    /// Trạng thái người dùng với problem
    /// </summary>
    public enum ProblemAccountState
    {
        /// <summary>
        /// Chưa xem
        /// </summary>
        None = 0,
        /// <summary>
        /// Đã xem
        /// </summary>
        Seen = 1,
        /// <summary>
        /// Đang làm
        /// </summary>
        Doing = 2,
        /// <summary>
        /// Đã nộp nhưng sai
        /// </summary>
        Wrong = 3,
        /// <summary>
        /// Đã giải
        /// </summary>
        Accepted = 4
    }
}
