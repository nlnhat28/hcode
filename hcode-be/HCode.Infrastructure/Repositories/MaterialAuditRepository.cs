using Dapper;
using HCode.Domain;
using System.Data;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository nhật ký nguyên vật liệu
    /// </summary>
    /// <typeparam name="MaterialAudit">Entity nhật ký nguyên vật liệu</typeparam>
    /// Created by: nlnhat (17/08/2023)
    public class MaterialAuditRepository : BaseRepository<MaterialAudit>, IMaterialAuditRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository nguyên vật liệu
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public MaterialAuditRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Đếm số lượng theo các năm
        /// </summary>
        /// <returns>Danh sách số lượng theo năm</returns>
        /// Created by: nlnhat (08/09/2023)
        public async Task<IEnumerable<CountByYearModel>> CountByYear()
        {
            var proc = $"{Procedure}CountByYear";

            var result = await _unitOfWork.Connection.QueryAsync<CountByYearModel>(
                proc, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
    }
    #endregion
}

