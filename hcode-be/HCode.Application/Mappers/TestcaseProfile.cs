using AutoMapper;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper testcase
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class TestcaseProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper testcase
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public TestcaseProfile() : base()
        {
            // Testcase to dto
            CreateMap<Testcase, TestcaseDto>()
            .ForMember(dest => dest.Inputs, opt =>
            {   // string to List<object>
                opt.MapFrom(src => AppHelper.ConvertToObjects(src.Inputs));
            })
            .ForMember(dest => dest.ExpectedOutput, opt =>
            {   // string to object
                opt.MapFrom(src => AppHelper.ConvertToObject(src.ExpectedOutput));
            });
        }
        #endregion
    }
}