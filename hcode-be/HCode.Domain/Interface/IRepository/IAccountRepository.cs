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
    }
}