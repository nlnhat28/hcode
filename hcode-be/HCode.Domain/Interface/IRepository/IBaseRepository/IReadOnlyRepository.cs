namespace HCode.Domain
{
    /// <summary>
    /// Giao diện repository chỉ đọc dữ liệu database
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (15/08/2023)
    public interface IReadOnlyRepository<TEntity>
    {
        /// <summary>
        /// Lấy tất cả đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng được lấy</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();
        /// <summary>
        /// Lấy đối tượng theo danh sách id
        /// </summary>
        /// <param name="ids">Danh sách id của đối tượng</param>
        /// <returns>Các đối tượng có id trong danh sách</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<IEnumerable<TEntity>> GetManyAsync(IEnumerable<Guid> ids);
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Id của đối tượng</param>
        /// <returns>Đối tượng được tìm thấy</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<TEntity?> GetAsync(Guid id);
        /// <summary>
        /// Lấy đối tượng theo id và account
        /// </summary>
        /// <param name="id">Id của đối tượng</param>
        /// <returns>Đối tượng được tìm thấy</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<TEntity?> GetAsync(Guid id, Guid accountId);
        /// <summary>
        /// Lọc đối tượng
        /// </summary>
        /// <param name="keySearch"></param>
        /// <param name="pagingModel"></param>
        /// <param name="sortModels"></param>
        /// <param name="filterModels"></param>
        /// <returns></returns>
        Task<FilterResultModel<TEntity>> FilterAsync(
            string? keySearch, PagingModel? pagingModel, List<SortModel>? sortModels, List<FilterModel>? filterModels, Guid? accountId);
        /// <summary>
        /// Lấy đối tượng được lọc
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetDataFilterAsync(string proc, object? param);
    }
}
