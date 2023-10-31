using Dapper;
using HCode.Domain;
using System.Data;
using System.Xml.Linq;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ConversionUnitRepository : BaseRepository<ConversionUnit>, IConversionUnitRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository đơn vị chuyển đổi
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ConversionUnitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách đơn vị chuyển đổi theo nguyên vật liệu
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Danh sách đơn vi chuyển đổi</returns>
        /// Created by: nlnhat (30/08/2023)
        public async Task<IEnumerable<ConversionUnit>> GetByMaterialId(Guid materialId)
        {
            var proc = $"{Procedure}GetByMaterialId";

            var param = new DynamicParameters();
            param.Add($"p_MaterialId", materialId);

            var result = await _unitOfWork.Connection.QueryAsync<ConversionUnit>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        #endregion
    }
}

