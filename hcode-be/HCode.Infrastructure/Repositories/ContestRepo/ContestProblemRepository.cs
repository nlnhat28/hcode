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
    public class ContestProblemRepository : BaseRepository<ContestProblem>, IContestProblemRepository
    {
        #region Properties
        /// <summary>
        /// Tên bảng
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public override string Table { get; set; } = "contest_problem";
        /// <summary>
        /// Tên view
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public override string View { get; set; } = "view_contest_problem"; 
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo repository testcase
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ContestProblemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        #endregion
    }
}

