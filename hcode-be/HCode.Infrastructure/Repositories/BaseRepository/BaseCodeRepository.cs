using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Triển khai repository cơ sở
    /// </summary>
    /// <typeparam name="TEntity">Đối tượng</typeparam>
    /// Created by: nlnhat (16/08/2023)
    public abstract class BaseCodeRepository<TEntity> : BaseRepository<TEntity>, IBaseCodeRepository<TEntity>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository cơ sở
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (16/08/2023)
        public BaseCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy đối tượng theo mã
        /// </summary>
        /// <param name="code">Mã đối tượng</param>
        /// <returns>Đối tượng có mã tương ứng</returns>
        public async Task<TEntity?> GetByCodeAsync(string code)
        {
            var proc = $"{Procedure}GetByCode";

            var param = new DynamicParameters();
            param.Add($"p_{Table}Code", code);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<TEntity>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Lấy đối tượng theo danh sách mã
        /// </summary>
        /// <param name="codes">Danh sách mã đối tượng</param>
        /// <returns>Danh sách đối tượng có mã tương ứng</returns>
        public async Task<IEnumerable<TEntity>> GetManyByCodeAsync(List<string>? codes)
        {
            if (codes?.Count > 0)
            {
                var proc = $"{Procedure}GetManyByCode";

                var param = new DynamicParameters();
                var codesJson = JsonSerializer.Serialize(codes);
                param.Add($"p_{Table}Codes", codesJson);

                var result = await _unitOfWork.Connection.QueryAsync<TEntity>(
                    proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

                return result;
            }
            return new List<TEntity>();
        }
        #endregion
    }
}