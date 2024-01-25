using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository contest
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ContestRepository : BaseCodeRepository<Contest>, IContestRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository testcase
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ContestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        
        // Get
        public override async Task<Contest?> GetAsync(Guid contestId, Guid accountId) 
        {
            var proc = $"{Procedure}Get";

            var param = new DynamicParameters();
            param.Add($"p_{TableId}", contestId);
            param.Add($"p_AccountId", accountId);

            using var multi = await _unitOfWork.Connection.QueryMultipleAsync(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            var contest = await multi.ReadFirstOrDefaultAsync<Contest>();

            if (contest != null)
            {
                contest.ContestProblems = (await multi.ReadAsync<ContestProblem>()).ToList();
            }

            return contest;

        }
        #endregion
    }
}

