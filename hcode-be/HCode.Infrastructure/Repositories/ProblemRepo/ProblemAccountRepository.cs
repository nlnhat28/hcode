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
    public class ProblemAccountRepository : BaseRepository<Parameter>, IProblemAccountRepository
    {
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
        #endregion
    }
}

