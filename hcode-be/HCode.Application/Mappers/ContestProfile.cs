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
        /// Hàm tạo mapper problem
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public ContestProfile() : base()
        {
            // Problem to Dto
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
            });

            // Dto to Problem
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
            });
        }
        #endregion
    }
}