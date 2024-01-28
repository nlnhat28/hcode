using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper contest
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class SubmissionProfile : BaseProfile<SubmissionDto, Submission>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper problem
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public SubmissionProfile() : base()
        {
            // Problem to Dto
            CreateMap<Submission, SubmissionDto>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateUtcToLocal(src.CreatedTime));
            });

            // Dto to Problem
            CreateMap<SubmissionDto, Submission>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                    AppHelper.ConvertDateLocalToUtc(src.CreatedTime));
            });
        }
        #endregion
    }
}