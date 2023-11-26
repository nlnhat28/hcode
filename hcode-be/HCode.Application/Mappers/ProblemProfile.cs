using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper problem
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class ProblemProfile : BaseProfile<ProblemDto, Problem>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper problem
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public ProblemProfile() : base()
        {
            CreateMap<Problem, ProblemDto>()
            .ForMember(dest => dest.IsNew, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.IsNew(src.CreatedTime, 7));
            })
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
        }
        #endregion
    }
}