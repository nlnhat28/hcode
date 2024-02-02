using Dapper;
using HCode.Domain;
using Org.BouncyCastle.Crypto.Operators;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository problem
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ProblemRepository : BaseCodeRepository<Problem>, IProblemRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository problem
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ProblemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        // Get
        public override async Task<Problem?> GetAsync(Guid problemId, Guid accountId) 
        {
            var proc = $"{Procedure}Get";

            var param = new DynamicParameters();
            param.Add($"p_{TableId}", problemId);
            param.Add($"p_AccountId", accountId);

            using var multi = await _unitOfWork.Connection.QueryMultipleAsync(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            var problem = await multi.ReadFirstOrDefaultAsync<Problem>();

            if (problem != null)
            {
                problem.Parameters = (await multi.ReadAsync<Parameter>()).ToList();
                problem.Testcases = (await multi.ReadAsync<Testcase>()).ToList();
            }

            return problem;

        }

        // Lấy mã lớn nhất
        public async Task<int> GetMaxCodeAsync(ProblemState state, Guid AccountId)
        {
            var proc = $"{Procedure}GetMaxCode";

            var param = new DynamicParameters();
            param.Add($"p_State", state);
            param.Add($"p_AccountId", AccountId);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<int>(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }

        // Lấy danh sách bài toán cho bài thi
        public async Task<IEnumerable<Problem>> GetForContestAsync(Guid accountId)
        {
            var proc = $"{Procedure}GetForContest";

            var param = new DynamicParameters();
            param.Add($"p_AccountId", accountId);

            var result = await _unitOfWork.Connection.QueryAsync<Problem>(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        #endregion
    }
}

