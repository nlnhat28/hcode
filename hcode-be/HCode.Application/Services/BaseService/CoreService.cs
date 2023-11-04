
using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace HCode.Application
{
    /// <summary>
    /// Service core
    /// </summary>
    public class CoreService : ICoreService
    {
        #region Fields
        /// <summary>
        /// Resource lưu thông báo
        /// </summary>
        /// Created by: nlnhat (13/07/2023)ks
        protected readonly IStringLocalizer<Resource> _resource;
        /// <summary>
        /// Mapper
        /// </summary>
        /// Created by: nlnhat (13/07/2023)
        protected readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service
        /// </summary>
        /// <param name="resource">Resource lưu thông báo</param>
        /// <param name="mapper">Mapper</param>
        /// Created by: nlnhat (13/07/2023)ks
        public CoreService(IStringLocalizer<Resource> resource, IMapper mapper)
        {
            _resource = resource;
            _mapper = mapper;
        } 
        #endregion
    }
}
