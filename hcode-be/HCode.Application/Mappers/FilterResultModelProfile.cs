using AutoMapper;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper FilterResultModel
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class FilterResultModelProfile<TEntityDto, TEntity> : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper FilterResultModel
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public FilterResultModelProfile()
        {
            CreateMap<FilterResultModel<TEntity>, FilterResultModel<TEntityDto>>();
            CreateMap<FilterResultModel<TEntityDto>, FilterResultModel<TEntity>>();
        }
        #endregion
    }
}