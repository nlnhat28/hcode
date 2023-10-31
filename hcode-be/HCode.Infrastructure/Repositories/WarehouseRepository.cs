using Dapper;
using HCode.Domain;
using Newtonsoft.Json;
using System.Data;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository kho
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository kho
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public WarehouseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy kho theo mã
        /// </summary>
        /// <param name="code">Mã kho</param>
        /// <returns>Kho có mã tương ứng</returns>
        public async Task<Warehouse> GetByCodeAsync(string code)
        {
            var proc = $"{Procedure}GetByCode";

            var param = new DynamicParameters();
            param.Add($"p_{Table}Code", code);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Warehouse>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
        /// <summary>
        /// Lấy kho theo danh sách mã
        /// </summary>
        /// <param name="codes">Danh sách mã kho</param>
        /// <returns>Danh sách kho có mã tương ứng</returns>
        public async Task<IEnumerable<Warehouse>> GetManyByCodeAsync(List<string>? codes)
        {
            if (codes?.Count > 0)
            {
                var proc = $"{Procedure}GetManyByCode";

                var param = new DynamicParameters();
                var codesJson = JsonConvert.SerializeObject(codes);
                param.Add($"p_{Table}Codes", codesJson);

                var result = await _unitOfWork.Connection.QueryAsync<Warehouse>(
                    proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                return result;
            }
            return new List<Warehouse>();
        }
        #endregion
    }
}

