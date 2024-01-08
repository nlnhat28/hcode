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
            // Problem to Dto
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
            })
            .ForMember(dest => dest.SolutionLanguage, opt =>
            {
                opt.MapFrom(src => src.SolutionLanguageId == null || src.JudgeId == null ? new Language() : 
                    new Language()
                    {
                        LanguageId = (Guid)src.SolutionLanguageId,
                        LanguageName = src.LanguageName,
                        JudgeId = (int)src.JudgeId
                    });
            })
            .ForMember(dest => dest.Testcases, opt =>
            {
                opt.Ignore();
            })
            .ForMember(dest => dest.Parameters, opt =>
            {
                opt.Ignore();
            });

            // Dto to Problem
            CreateMap<ProblemDto, Problem>()
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
            .ForMember(dest => dest.SolutionLanguageId, opt =>
            {
                opt.MapFrom(src => src.SolutionLanguage != null ? src.SolutionLanguage.LanguageId : Guid.Empty);
            })
            .ForMember(dest => dest.Testcases, opt =>
            {
                opt.Ignore();
            })
            .ForMember(dest => dest.Parameters, opt =>
            {
                opt.Ignore();
            });
        }
        #endregion
    }
}