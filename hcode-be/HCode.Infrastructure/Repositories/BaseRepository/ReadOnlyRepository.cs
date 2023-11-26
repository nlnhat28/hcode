﻿using System.Data;
using System.Text.Json;
using Dapper;
using HCode.Domain;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Triển khai giao diện cơ sở repository chỉ đọc dữ liệu từ database
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public abstract class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
    {
        #region Fields
        /// <summary>
        /// Unit of work
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        protected readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Tên bảng
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public virtual string Table { get; set; } = typeof(TEntity).Name;
        /// <summary>
        /// Tên cột khoá chính 
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public virtual string TableId { get; set; } = $"{typeof(TEntity).Name}Id";
        /// <summary>
        /// Tên stored procedure
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public virtual string Procedure { get; set; } = $"proc_{typeof(TEntity).Name}_";
        /// <summary>
        /// Tên stored procedure
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        public virtual string View { get; set; } = $"view_{typeof(TEntity).Name}";
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo repository phòng ban
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (18/07/2023)
        public ReadOnlyRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                var proc = $"{Procedure}GetAll";

                var result = await _unitOfWork.Connection.QueryAsync<TEntity>(
                    proc, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch
            {
                var sql = $"SELECT * FROM {View}";
                var result = await _unitOfWork.Connection.QueryAsync<TEntity>(
                    sql, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);

                return result;
            }

        }
        /// <summary>
        /// Lấy đối tượng theo danh sách id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>Đối tượng có id trong danh sách</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<IEnumerable<TEntity>> GetManyAsync(IEnumerable<Guid> ids)
        {
            try
            {
                var proc = $"{Procedure}GetMany";

                var idsJson = JsonSerializer.Serialize(ids);

                var param = new DynamicParameters();
                param.Add($"p_{TableId}s", idsJson);

                var result = await _unitOfWork.Connection.QueryAsync<TEntity>(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch
            {
                var sql = $"SELECT * FROM {View} WHERE {TableId} IN @ids";

                var param = new DynamicParameters();
                param.Add("ids", ids);

                var result = await _unitOfWork.Connection.QueryAsync<TEntity>(
                    sql, param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);

                return result;
            }
        }
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bản ghi có id được truy vấn</returns>
        /// Created by: nlnhat (16/08/2023)
        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            try
            {
                var proc = $"{Procedure}Get";

                var param = new DynamicParameters();
                param.Add($"p_{TableId}", id);

                var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<TEntity>(
                    proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch
            {
                var sql = $"SELECT * FROM {View} WHERE {TableId} = @id";

                var param = new DynamicParameters();
                param.Add("id", id);

                var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<TEntity>(
                    sql, param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);

                return result;
            }
        }
        /// <summary>
        /// Lọc nguyên vật liệu (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="pagingModel">Các thuộc tính phân trang</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <param name="accountId">Id account</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<FilterResultModel<TEntity>> FilterAsync(
            string? keySearch, PagingModel? pagingModel, List<SortModel>? sortModels, List<FilterModel>? filterModels, Guid? accountId)
        {
            var proc = $"{Procedure}Filter";

            // Tạo param
            var param = new DynamicParameters();

            var querySearch = InfraHelper.GenerateQuerySearch<TEntity>(keySearch);
            param.Add("p_QuerySearch", querySearch);

            var querySort = InfraHelper.GenerateQuerySort<TEntity>(sortModels);
                param.Add("p_QuerySort", querySort);

            var queryFilter = InfraHelper.GenerateQueryFilter<TEntity>(filterModels);
            param.Add("p_QueryFilter", queryFilter);

            param.Add("p_AccountId", accountId);

            // Lấy data
            var data = await GetDataFilterAsync(proc, param);

            // Phân trang
            var totalRecord = data?.Count() ?? 0;

            // Phân trang
            data = InfraHelper.Paging(data, ref pagingModel);

            var result = new FilterResultModel<TEntity>()
            {
                TotalRecord = totalRecord,
                PagingModel = pagingModel,
                Data = data
            };

            return result;
        }

        // Lấy đối tượng được lọc
        public virtual async Task<IEnumerable<TEntity>> GetDataFilterAsync(string proc, object? param)
        {
            var data = await _unitOfWork.Connection.QueryAsync<TEntity>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            return data;
        }
        #endregion
    }
}
