using Dapper;
using HCode.Domain;
using System.Data;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    public class BaseMasterDetails<TMaster, TDetail> : BaseRepository<TMaster>, IBaseMasterDetails<TMaster, TDetail> 
        where TMaster : IHasEntityId, IMaster<TDetail>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository cơ sở
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (16/08/2023)
        public BaseMasterDetails(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        #endregion

        #region Methods
        // Lọc đối tượng master detail
        public async Task<FilterResultModel<TMaster>> FilterMasterAsync(string? keySearch, PagingModel? pagingModel, List<SortModel> sortModels, List<FilterModel> filterModels)
        {
            var proc = $"{Procedure}Export";

            var param = new DynamicParameters();
            param.Add("p_KeySearch", keySearch);

            if (sortModels != null)
            {
                var querySort = InfraHelper.GenerateQuerySort<TMaster>(sortModels);
                param.Add("p_QuerySort", querySort);
            }
            else param.Add("p_QuerySort", string.Empty);

            if (filterModels != null)
            {
                var queryFilter = InfraHelper.GenerateQueryFilter<TMaster>(filterModels);
                param.Add("p_QueryFilter", queryFilter);
            }
            else param.Add("p_QueryFilter", string.Empty);

            var dictionary = new Dictionary<Guid, TMaster>();
            var masters = await _unitOfWork.Connection.QueryAsync<TMaster, TDetail, TMaster>(proc, (master, detail) =>
            {
                if (!dictionary.TryGetValue(master.Id, out var currentMaster))
                {
                    currentMaster = master;
                    dictionary.Add(currentMaster.Id, currentMaster);
                }
                currentMaster.Details ??= new List<TDetail>();
                if (detail != null)
                    currentMaster.Details.Add(detail);

                return currentMaster;
            },
            param: param,
            transaction: _unitOfWork.Transaction,
            splitOn: "ConversionUnitId",
            commandType: CommandType.StoredProcedure);

            var data = dictionary.Values.ToList();
            var totalRecord = param.Get<int>("p_TotalRecord");
            var allRecord = param.Get<int>("p_AllRecord");

            data = InfraHelper.Paging(data, ref pagingModel).ToList();

            var pageNumber = param.Get<int>("p_PageNumberOut");

            var result = new FilterResultModel<TMaster>()
            {
                TotalRecord = totalRecord,
                AllRecord = allRecord,
                PagingModel = pagingModel,
                Data = data
            };

            return result;
        } 
        #endregion

    }
}
