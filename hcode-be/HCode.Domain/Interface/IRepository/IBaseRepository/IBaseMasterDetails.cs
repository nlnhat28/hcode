
namespace HCode.Domain
{
    public interface IBaseMasterDetails<TMaster, TDetail> : IBaseRepository<TMaster>
    {
        /// <summary>
        /// Lọc đối tượng master detail
        /// </summary>
        /// <param name="keySearch"></param>
        /// <param name="pagingModel"></param>
        /// <param name="sortModels"></param>
        /// <param name="filterModels"></param>
        /// <returns></returns>
        Task<FilterResultModel<TMaster>> FilterMasterAsync(string? keySearch, PagingModel? pagingModel, List<SortModel> sortModels, List<FilterModel> filterModels);
    }
}
