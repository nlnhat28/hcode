using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository ProblemAccountRepository
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ProblemAccountRepository : BaseRepository<ProblemAccount>, IProblemAccountRepository
    {
        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo repository ProblemAccountRepository
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ProblemAccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods

        // Cập nhật theo ProblemId và AccountId
        public async Task<int> AuditProblemAccountAsync(ProblemAccount problemAccount)
        {
            var proc = $"{Procedure}Audit";

            var param = InfraHelper.GetParamFromEntity(problemAccount);

            var result = await _unitOfWork.Connection.ExecuteAsync(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
        #endregion
    }
}

