using HCode.Application;
using HCode.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HCode.Api
{
    /// <summary>
    /// Controller language
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class LanguageController : ReadOnlyController<Language, Language>
    {
        #region Fields
        /// <summary>
        /// Service language
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly ILanguageService _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        /// <param name="service">Service language</param>
        /// Created by: nlnhat (17/08/2023)
        public LanguageController(ILanguageService service) : base(service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        #endregion
    }
}