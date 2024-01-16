using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository parameter
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ParameterRepository : BaseCodeRepository<Parameter>, IParameterRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository parameter
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ParameterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        #endregion
    }
}

