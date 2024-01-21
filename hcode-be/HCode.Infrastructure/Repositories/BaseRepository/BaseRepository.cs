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
            try
            {
                var proc = $"{Procedure}Insert";

                var param = InfraHelper.GetParamFromEntity(entity);
                param.Add($"p_{TableId}Out", dbType: DbType.Guid, direction: ParameterDirection.Output);

                _ = await _unitOfWork.Connection.ExecuteAsync(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

                var result = param.Get<Guid>($"p_{TableId}Out");
                return result;
            }
            catch
            {
                var (sql, param) = InfraHelper.ScriptInsert(entity);

                var id = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<string>(
                    sql, param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);

                var result = id == null ? Guid.Empty : Guid.Parse(id);

                return result;
            }
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
                try
                {
                    var proc = $"{Procedure}InsertMany";

                    var entitiesJson = JsonSerializer.Serialize(entities);

                    var param = new DynamicParameters();
                    param.Add($"p_{Table}s", entitiesJson);

                    var result = await _unitOfWork.Connection.ExecuteAsync(
                        proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch
                {
                    var scripts = new List<string>();
                    var parames = new DynamicParameters();
                    var index = 1;

                    foreach (var entity in entities)
                    {
                        var (script, param) = InfraHelper.ScriptInsert(entity, index, false);
                        scripts.Add(script);
                        parames.AddDynamicParams(param);
                        index++;
                    }

                    var sql = string.Join("\n", scripts);

                    var result = await _unitOfWork.Connection.ExecuteAsync(
                        sql, parames, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
                    return result;
                }
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
            try
            {
                var proc = $"{Procedure}Update";

                var param = InfraHelper.GetParamFromEntity(entity);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch
            {
                var (sql, param) = InfraHelper.ScriptUpdate(entity);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    sql, param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);

                return result;
            }
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
                try
                {
                    var proc = $"{Procedure}UpdateMany";

                    var entitiesJson = JsonSerializer.Serialize(entities);

                    var param = new DynamicParameters();
                    param.Add($"p_{Table}s", entitiesJson);

                    var result = await _unitOfWork.Connection.ExecuteAsync(
                        proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch
                {
                    var scripts = new List<string>();
                    var parames = new DynamicParameters();
                    var index = 1;

                    foreach (var entity in entities)
                    {
                        var (script, param) = InfraHelper.ScriptUpdate(entity, index);
                        scripts.Add(script);
                        parames.AddDynamicParams(param);
                        index++;
                    }

                    var sql = string.Join("\n", scripts);

                    var result = await _unitOfWork.Connection.ExecuteAsync(
                        sql, parames, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
                    return result;
                }
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
            try
            {
                var proc = $"{Procedure}Delete";

                var param = new DynamicParameters();
                param.Add($"p_{TableId}", id);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch
            {
                var sql = $"DELETE FROM {Table} WHERE {TableId} = @id";

                var param = new DynamicParameters();
                param.Add("id", id);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    sql, param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
                return result;
            }

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
                try
                {
                    var proc = $"{Procedure}DeleteMany";

                    var idsJson = JsonSerializer.Serialize(ids);

                    var param = new DynamicParameters();
                    param.Add($"p_{TableId}s", idsJson);

                    var result = await _unitOfWork.Connection.ExecuteAsync(
                        proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch
                {
                    var sql = $"DELETE FROM {Table} WHERE {TableId} IN @ids";

                    var param = new DynamicParameters();
                    param.Add("ids", ids);

                    var result = await _unitOfWork.Connection.ExecuteAsync(
                        sql, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }

            return 0;
        }
        /// <summary>
        /// Xoá tât cả đối tượng
        /// </summary>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (15/08/2023)
        public async Task<int> DeleteAllAsync()
        {
            try
            {
                var proc = $"{Procedure}DeleteAll";

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    proc, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch
            {
                var sql = $"DELETE FROM {Table}";

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    sql, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
                return result;
            }
        }

        // Replace many
        public async Task<int> ReplaceManyAsync(IEnumerable<TEntity>? entities, Guid masterId, string masterColumn)
        {
            if (entities?.Count() > 0)
            {
                var scripts = new List<string>();
                var parames = new DynamicParameters();
                var index = 1;

                foreach (var entity in entities)
                {
                    var (script, param) = InfraHelper.ScriptInsert(entity, index, false);
                    scripts.Add(script);
                    parames.AddDynamicParams(param);
                    index++;
                }

                var sql = $"DELETE FROM {Table} WHERE {masterColumn} = @p_MasterId;\n";
                parames.Add("p_MasterId", masterId);

                sql += string.Join("\n", scripts);

                var result = await _unitOfWork.Connection.ExecuteAsync(
                    sql, parames, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
                return result;
            };

            return 0;
        }
        #endregion
    }
}