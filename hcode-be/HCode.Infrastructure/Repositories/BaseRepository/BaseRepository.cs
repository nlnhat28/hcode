using Dapper;
using HCode.Domain;
using Newtonsoft.Json;
using System.Data;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Triển khai repository cơ sở
    /// </summary>
    /// <typeparam name="TEntity">Đối tượng</typeparam>
    /// Created by: nlnhat (16/08/2023)
    public abstract class BaseRepository<TEntity> : ReadOnlyRepository<TEntity>, IBaseRepository<TEntity>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository cơ sở
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (16/08/2023)
        public BaseRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        #endregion

        #region Methods
        /// <summary>
        /// Tạo mới 1 bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng mới</param>
        /// <returns>Id của bản ghi mới</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<Guid> InsertAsync(TEntity entity)
        {
            var proc = $"{Procedure}Insert";

            var param = InfrastructureHelper.GetParamFromEntity(entity);
            param.Add($"p_{TableId}Out", dbType: DbType.Guid, direction: ParameterDirection.Output);

            _ = await _unitOfWork.Connection.ExecuteAsync(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            var result = param.Get<Guid>($"p_{TableId}Out");
            return result;
        }
        /// <summary>
        /// Tạo mới nhiều bản ghi
        /// </summary>
        /// <param name="entities">Danh sách đối tượng mới</param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<int> InsertManyAsync(IEnumerable<TEntity>? entities)
        {

            if (entities?.Count() > 0)
            {
                var proc = $"{Procedure}InsertMany";

                var entitiesJson = JsonConvert.SerializeObject(entities);

                var param = new DynamicParameters();
                param.Add($"p_{Table}s", entitiesJson);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                return result;
            }

            return 0;
        }
        /// <summary>
        /// Cập nhật 1 đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng muốn cập nhật</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<int> UpdateAsync(TEntity entity)
        {
            var proc = $"{Procedure}Update";

            var param = InfrastructureHelper.GetParamFromEntity(entity);

            var result = await _unitOfWork.Connection.ExecuteAsync(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
        /// <summary>
        /// Cập nhật nhiều đối tượng
        /// </summary>
        /// <param name="entities">Danh sách đối tượng muốn cập nhật</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<int> UpdateManyAsync(IEnumerable<TEntity>? entities)
        {
            if (entities?.Count() > 0)
            {
                var proc = $"{Procedure}UpdateMany";

                var entitiesJson = JsonConvert.SerializeObject(entities);

                var param = new DynamicParameters();
                param.Add($"p_{Table}s", entitiesJson);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                return result;
            };

            return 0;
        }
        /// <summary>
        /// Xoá 1 đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng muốn xoá</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<int> DeleteAsync(Guid id)
        {
            var proc = $"{Procedure}Delete";

            var param = new DynamicParameters();
            param.Add($"p_{TableId}", id);

            var result = await _unitOfWork.Connection.ExecuteAsync(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
        /// <summary>
        /// Xoá nhiều đối tượng theo id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<int> DeleteManyAsync(IEnumerable<Guid>? ids)
        {

            if (ids?.Count() > 0)
            {
                var proc = $"{Procedure}DeleteMany";

                var idsJson = JsonConvert.SerializeObject(ids);

                var param = new DynamicParameters();
                param.Add($"p_{TableId}s", idsJson);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                return result;
            }

            return 0;
        }
        #endregion
    }
}