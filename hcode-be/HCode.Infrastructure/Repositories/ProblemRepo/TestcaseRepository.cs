using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository testcase
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class TestcaseRepository : BaseRepository<Testcase>, ITestcaseRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository testcase
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public TestcaseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        #endregion
    }
}

