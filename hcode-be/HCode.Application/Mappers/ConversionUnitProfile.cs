using AutoMapper;
using HCode.Domain;

namespace HCode.Application   
{
    /// <summary>
    /// Tạo mapper đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class ConversionUnitProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper đơn vị chuyển đổi
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public ConversionUnitProfile()
        {
            CreateMap<ConversionUnit, ConversionUnitDto>()
            .ForMember(dest => dest.OperatorName, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.GetOperatorName(src.Operator));
            })
            .ForMember(dest => dest.EditMode, opt =>
            {
                opt.MapFrom(src => EditMode.NoEdit);
            });
            CreateMap<ConversionUnit, ConversionUnitExcelDto>()
            .ForMember(dest => dest.OperatorName, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.GetOperatorName(src.Operator));
            })
            .ForMember(dest => dest.Description, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.GetConversionUnitDescription(src.UnitName, src.DestinationUnitName, src.Operator, src.Rate));
            });

            CreateMap<ConversionUnitDto, ConversionUnit>();
            CreateMap<ConversionUnitImportDto, ConversionUnitDto>();
        }
        #endregion
    } 
}