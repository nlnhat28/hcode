using AutoMapper;
using HCode.Application;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper đơn vị 
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class UnitProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper đơn vị
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public UnitProfile()
        {
            CreateMap<Unit, UnitDto>().ReverseMap();
        }
        #endregion
    } 
}