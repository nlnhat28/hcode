using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Giao diện service chỉ đọc dữ liệu
    /// </summary>
    /// <typeparam name="TEntityDto">Dto thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public interface IReadOnlyService<TEntityDto, TEntity> : ICoreService
    {
        /// <summary>
        /// Lấy danh sách đối tượng
        /// </summary>
        /// <returns>Danh sách dto đối tượng</returns>
        /// Created by: nlnhat (13/07/2023)
        Task<IEnumerable<TEntityDto>> GetAllAsync();
        /// <summary>
        /// Lấy đối tượng theo danh sách id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>Đối tượng có id trong danh sách</returns>
        /// Created by: nlnhat (18/07/2023)
        Task<IEnumerable<TEntityDto>> GetManyAsync(IEnumerable<Guid> ids);
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Dto đối tượng có id được truy vấn</returns>
        /// Created by: nlnhat (13/07/2023)
        Task<TEntityDto> GetAsync(Guid id);
        /// <summary>
        /// Lọc (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="pagingModel">Các thuộc tính phân trang</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        Task<FilterResultModel<TEntityDto>> FilterAsync(
            string? keySearch, PagingModel? pagingModel, List<SortModel>? sortModels, List<FilterModel>? filterModels);
    }
}
