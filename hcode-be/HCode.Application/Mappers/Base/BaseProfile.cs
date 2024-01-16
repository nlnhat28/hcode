using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Localization;

namespace HCode.Application
{
    /// <summary>
    /// Mapper base
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class BaseProfile<TEntityDto, TEntity> : Profile where TEntityDto : BaseDto where TEntity : BaseEntity
    {
        #region Fields
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo mapper base
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public BaseProfile()
        {
            // Entity -> Dto
            CreateMap<TEntity, TEntityDto>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.CreatedTime));
            })
            .ForMember(dest => dest.ModifiedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.ModifiedTime));
            });

            // Dto -> Entity
            CreateMap<TEntityDto, TEntity>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                 AppHelper.ConvertDateLocalToUtc(src.CreatedTime));
            })
            .ForMember(dest => dest.ModifiedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateLocalToUtc(src.ModifiedTime));
            });

            // Account -> MaterialExcelDto
            // CreateMap<Account, MaterialExcelDto>()
            //.ForMember(dest => dest.TimeUnitName, opt =>
            //{
            //    opt.MapFrom(src =>
            //        ApplicationHelper.GetTimeUnitName(src.TimeUnit));
            //})
            //.ForMember(dest => dest.ConversionUnits, opt =>
            //{
            //    opt.Ignore();
            //});
        }
        #endregion
    }
}