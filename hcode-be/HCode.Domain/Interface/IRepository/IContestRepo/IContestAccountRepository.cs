namespace HCode.Domain
{
    /// <summary>
    /// Repo contest account
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IContestAccountRepository : IBaseRepository<ContestAccount>
    {
        /// <summary>
        /// Cập nhật theo ContestId và AccountId
        /// </summary>
        /// <param name="contestAccount">Thông tin mới</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<int> AuditContestAccountAsync(ContestAccount contestAccount);
    }
}