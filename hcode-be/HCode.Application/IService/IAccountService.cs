using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Service account
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IAccountService : IBaseService<AccountDto, Account>
    {
        /// <summary>
        /// Cập nhật trạng thái đã xác thực
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="res"></param>
        /// <returns></returns>
       Task UpdateVerifyAsync(Guid accountId, ServerResponse res);
        /// <summary>
        /// Cập nhật lại password
        /// </summary>
        /// <param name="accountDto">Id tài khoản</param>
        /// <param name="res">Mật khẩu mới</param>
        /// <returns></returns>
        Task ChangePasswordAsync(AccountDto accountDto, ServerResponse res);
    }
}
