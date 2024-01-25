using AutoMapper;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper testcase
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class ContestProblemProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper testcase
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public ContestProblemProfile() : base()
        {
            CreateMap<ContestProblem, ContestProblemDto>().ReverseMap();
        }
        #endregion
    }
}