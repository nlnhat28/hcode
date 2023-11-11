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
    }
}
