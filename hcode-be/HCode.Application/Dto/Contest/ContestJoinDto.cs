
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Param join contest
    /// </summary>
    public class ContestJoinDto
    {
        #region Properties
        /// <summary>
        /// Bài thi tài khoản
        /// </summary>
        public Guid ContestId { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? Password { get; set; }
        #endregion
    }
}