namespace HCode.Domain
{
    /// <summary>
    /// Repo ProblemAccount
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IProblemAccountRepository : IBaseRepository<ProblemAccount>
    {
        /// <summary>
        /// Cập nhật theo ProblemId và AccountId
        /// </summary>
        /// <param name="entity">Thông tin mới</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<int> UpdateAsync(TEntity entity);
    }
}