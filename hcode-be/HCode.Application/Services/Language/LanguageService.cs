using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Localization;
namespace HCode.Application
{
    /// <summary>
    /// Triển khai service language
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class LanguageService : ReadOnlyService<Language, Language>, ILanguageService
    {
        #region Field
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service
        /// </summary>
        /// <param name="repository">Repository language</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map đối tượng</param>
        /// Created by: nlnhat (17/08/2023)
        public LanguageService(ILanguageRepository repository, IStringLocalizer<Resource> resource, 
                               IMapper mapper, IAuthService authService)
                         : base(repository, resource, mapper, authService)
        {
        }
        #endregion
    }
}