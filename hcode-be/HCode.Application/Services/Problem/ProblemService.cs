using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
namespace HCode.Application
{
    /// <summary>
    /// Triển khai service problem
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class ProblemService : BaseService<ProblemDto, Problem>, IProblemService
    {
        #region Fields
        /// <summary>
        /// Repo problem
        /// </summary>
        private new readonly IProblemRepository _repository;
        /// <summary>
        /// Repo vai trò
        /// </summary>
        private readonly IRoleRepository _roleRepository;
        /// <summary>
        /// Cache
        /// </summary>
        /// Created by: nlnhat (13/07/2023
        protected readonly IMemoryCache _cache;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service
        /// </summary>
        /// <param name="repository">Repository account</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map đối tượng</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public ProblemService(IProblemRepository repository, IRoleRepository roleRepository,
                           IStringLocalizer<Resource> resource, IMapper mapper, IAuthService authService,
                           IUnitOfWork unitOfWork, IMemoryCache cache)
                         : base(repository, resource, mapper, unitOfWork, authService)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _cache = cache;
        }
        #endregion
    }
}