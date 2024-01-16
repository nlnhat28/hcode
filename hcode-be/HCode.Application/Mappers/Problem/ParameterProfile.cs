using AutoMapper;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper parameter
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class ParameterProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper parameter
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public ParameterProfile() : base()
        {
            CreateMap<Parameter, ParameterDto>().ReverseMap();
        }
        #endregion
    }
}