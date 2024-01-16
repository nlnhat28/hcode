namespace HCode.Domain
{
    /// <summary>
    /// Repo Problem
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IProblemRepository : IBaseCodeRepository<Problem>
    {
        /// <summary>
        /// Lấy mã lớn nhất
        /// </summary>
        /// <param name="state">Công khai hay riêng tư</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<int> GetMaxCodeAsync(ProblemState state, Guid accountId);
        /// <summary>
        /// Lấy danh sách bài toán cho bài thi
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<IEnumerable<Problem>> GetForContestAsync(Guid accountId);
    }
}