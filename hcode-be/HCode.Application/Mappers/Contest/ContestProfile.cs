using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper contest
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class ContestProfile : BaseProfile<ContestDto, Contest>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper Contest
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public ContestProfile() : base()
        {
            // Contest to Dto
            CreateMap<Contest, ContestDto>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.CreatedTime));
            })
            .ForMember(dest => dest.ModifiedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.ModifiedTime));
            })
            .ForMember(dest => dest.StartTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.StartTime));
            })
            .ForMember(dest => dest.EndTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.EndTime));
            });

            // Dto to Contest
            CreateMap<ContestDto, Contest>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateLocalToUtc(src.CreatedTime));
            })
            .ForMember(dest => dest.ModifiedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateLocalToUtc(src.ModifiedTime));
            })
            .ForMember(dest => dest.StartTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateLocalToUtc(src.StartTime));
            })
            .ForMember(dest => dest.EndTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateLocalToUtc(src.EndTime));
            });
        }
        #endregion
    }
}