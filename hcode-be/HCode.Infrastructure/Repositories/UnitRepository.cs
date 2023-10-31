using Dapper;
using HCode.Domain;
using Newtonsoft.Json;
using System.Data;
using System.Reflection.Emit;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository đơn vị tính
    /// </summary>
    /// <typeparam name="Unit">Entity đơn vị tính</typeparam>
    /// Created by: nlnhat (17/08/2023)
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository đơn vị tính
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public UnitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy đơn vị tính theo tên
        /// </summary>
        /// <param name="name">Tên đơn vị tính</param>
        /// <returns>Đơn vị có tên tương ứng</returns>
        public async Task<Unit> GetByNameAsync(string name)
        {
            var proc = $"{Procedure}GetByName";

            var param = new DynamicParameters();
            param.Add($"p_{Table}Name", name);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Unit>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Lấy nhiều đơn vị tính theo tên
        /// </summary>
        /// <param name="names">Danh sách tên đơn vị tính</param>
        /// <returns>Danh sách đơn vị có tên tương ứng</returns>
        public async Task<IEnumerable<Unit>> GetManyByNameAsync(List<string> names)
        {
            if (names?.Count > 0)
            {
                var proc = $"{Procedure}GetManyByName";

                var param = new DynamicParameters();
                var namesJson = JsonConvert.SerializeObject(names);
                param.Add($"p_{Table}Names", namesJson);

                var result = await _unitOfWork.Connection.QueryAsync<Unit>(
                    proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

                return result;
            }
            return new List<Unit>();
        }
        /// <summary>
        /// Lấy tất cả id của đơn vị tính
        /// </summary>
        /// <returns>Danh sách tất cả id đơn vị tính</returns>
        public async Task<IEnumerable<Guid>> GetAllIdAsync()
        {
            var proc = $"{Procedure}GetAllId";

            var result = await _unitOfWork.Connection.QueryAsync<Guid>(
                proc, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        #endregion
    }
}

