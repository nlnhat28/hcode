using AutoMapper;
using HCode.Application;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper kho
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class WarehouseProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper kho
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
        }
        #endregion
    } 
}