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
    public class ContestProblemAccountRepository : BaseRepository<ContestProblem>, IContestProblemRepository
    {

        /// <summary>
        /// Tên bảng
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public override string Table { get; set; } = "contest_problem_account";
        /// <summary>
        /// Tên cột khoá chính 
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public override string TableId { get; set; } = $"{typeof(ContestProblemAccount).Name}Id";
        /// <summary>
        /// Tên stored procedure
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public override string Procedure { get; set; } = $"proc_{typeof(ContestProblemAccount).Name}_";
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
        #endregion
    }
}

