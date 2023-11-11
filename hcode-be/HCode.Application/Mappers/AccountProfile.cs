using AutoMapper;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class AccountProfile : Profile
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper tài khoản
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public AccountProfile()
        {
            CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.ConvertDateUtcToLocal(src.CreatedTime));
            })
            .ForMember(dest => dest.ModifiedTime, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.ConvertDateUtcToLocal(src.ModifiedTime));
            });

            // AccountDto -> Account
            CreateMap<AccountDto, Account>()
            .ForMember(dest => dest.CreatedTime, opt =>
            {
                opt.MapFrom(src =>
                 ApplicationHelper.ConvertDateLocalToUtc(src.CreatedTime));
            })
            .ForMember(dest => dest.ModifiedTime, opt =>
            {
                opt.MapFrom(src =>
                    ApplicationHelper.ConvertDateLocalToUtc(src.ModifiedTime));
            });

            // Account -> MaterialExcelDto
           // CreateMap<Account, MaterialExcelDto>()
           //.ForMember(dest => dest.TimeUnitName, opt =>
           //{
           //    opt.MapFrom(src =>
           //        ApplicationHelper.GetTimeUnitName(src.TimeUnit));
           //})
           //.ForMember(dest => dest.ConversionUnits, opt =>
           //{
           //    opt.Ignore();
           //});
        }
        #endregion
    }
}