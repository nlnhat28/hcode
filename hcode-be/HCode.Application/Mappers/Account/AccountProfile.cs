using HCode.Domain;
using Microsoft.Extensions.Localization;

namespace HCode.Application
{
    /// <summary>
    /// Tạo mapper account
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class AccountProfile : BaseProfile<AccountDto, Account>
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo mapper tài khoản
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        public AccountProfile() : base()
        {
            CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.Password, opt =>
            {
                opt.Ignore();
            })
            .ForMember(dest => dest.Salt, opt =>
            {
                opt.Ignore();
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