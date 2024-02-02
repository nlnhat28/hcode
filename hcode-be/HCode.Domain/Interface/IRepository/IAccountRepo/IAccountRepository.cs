namespace HCode.Domain
{
    /// <summary>
    /// Repo auth
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IAccountRepository : IBaseRepository<Account>
    {
        /// <summary>
        /// Lấy account theo username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns></returns>
        Task<Account?> GetByUsernameAsync(string? username);
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns></returns>
        Task<Account?> LoginAsync(string? username, string? password);
        /// <summary>
        /// Cập nhật trạng thái đã xác thực
        /// </summary>
        /// <param name="accountIds">Id của account</param>
        /// <returns></returns>
        Task<int> UpdateVerifiedAsync(IEnumerable<Guid> accountIds);
        /// <summary>
        /// Cập nhật lại password
        /// </summary>
        /// <param name="accountId">Id tài khoản</param>
        /// <param name="newPassword">Mật khẩu mới</param>
        /// <param name="salt">Salt</param>
        /// <returns></returns>
        Task<int> UpdatePasswordAsync(Guid accountId, string newPassword, string salt);
    }
}