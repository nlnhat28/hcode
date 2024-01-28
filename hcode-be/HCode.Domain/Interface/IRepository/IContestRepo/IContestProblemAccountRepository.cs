namespace HCode.Domain
{
    /// <summary>
    /// Repo contest problem account
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IContestProblemAccountRepository : IBaseRepository<ContestProblemAccount>
    {
        /// <summary>
        /// Lấy id theo ContestProblemId và AccountId
        /// </summary>
        /// <param name="contestProblemId"></param>
        /// <param name="accoountId"></param>
        /// <returns></returns>
        Task<ContestProblemAccount?> GetByConstraintAsync(Guid contestProblemId, Guid accoountId);
    }
}