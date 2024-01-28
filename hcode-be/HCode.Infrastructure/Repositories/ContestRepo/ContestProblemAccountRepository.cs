using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository contest problem account
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ContestProblemAccountRepository : BaseRepository<ContestProblemAccount>, IContestProblemAccountRepository
    {
        #region Properties
        /// <summary>
        /// Tên bảng
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public override string Table { get; set; } = "contest_problem_account";
        /// <summary>
        /// Tên view
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public override string View { get; set; } = "view_contest_problem_account";
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo repository testcase
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ContestProblemAccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        // Lấy id
        public async Task<ContestProblemAccount?> GetByConstraintAsync(Guid contestProblemId, Guid accoountId)
        {
            var sql = $@"SELECT {TableId} FROM {Table} WHERE ContestProblemId = @p_ContestProblemId AND AccountId = @p_AccountId";

            var param = new DynamicParameters();
            param.Add("p_ContestProblemId", contestProblemId);
            param.Add("p_AccountId", accoountId);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<ContestProblemAccount>(
                sql, param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);

            return result;
        }
        #endregion
    }
}

