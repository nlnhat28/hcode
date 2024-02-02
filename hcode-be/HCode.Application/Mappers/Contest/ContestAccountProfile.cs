using AutoMapper;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper contest
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class ContestAccountProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper problem
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public ContestAccountProfile()
        {
            // Problem to Dto
            CreateMap<ContestAccount, ContestAccountDto>()
            .ForMember(dest => dest.StartTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.StartTime));
            });

            // Dto to Problem
            CreateMap<ContestAccountDto, ContestAccount>()
            .ForMember(dest => dest.StartTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateLocalToUtc(src.StartTime));
            });
        }
        #endregion
    }
}