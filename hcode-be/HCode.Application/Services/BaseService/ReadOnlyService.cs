using AutoMapper;
using Microsoft.Extensions.Localization;
using HCode.Domain;
using System.Reflection;
using AutoMapper.Configuration.Annotations;

namespace HCode.Application
{
    /// <summary>
    /// Service cơ sở chỉ đọc dữ liệu
    /// </summary>
    /// <typeparam name="TEntityDto">Dto thực thể</typeparam>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023
    public abstract class ReadOnlyService<TEntityDto, TEntity> : CoreService, IReadOnlyService<TEntityDto, TEntity>
    {
        #region Fields
        /// <summary>
        /// Repository chỉ đọc dữ liệu từ database
        /// </summary>
        /// Created by: nlnhat (13/07/2023)ks
        protected readonly IReadOnlyRepository<TEntity> _repository;
        /// <summary>
        /// Service auth
        /// </summary>
        private readonly IAuthService _authService;
        #endregion

        #region Properties
        /// <summary>
        /// Tên Entity
        /// </summary>
        public virtual string EntityName { get; set; } = typeof(TEntity).Name;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service chỉ đọc
        /// </summary>
        /// <param name="repository">Repository chỉ đọc dữ liệu</param>
        /// <param name="resource">Resource lưu thông báo</param>
        /// <param name="mapper">Mapper</param>
        /// Created by: nlnhat (13/07/2023)ks
        public ReadOnlyService(
            IReadOnlyRepository<TEntity> repository,
            IStringLocalizer<Resource> resource,
            IMapper mapper,
            IAuthService authService) : base(resource, mapper)
        {
            _repository = repository;
            _authService = authService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách toàn bộ đối tượng
        /// </summary>
        /// <returns>Danh sách dto nhân viên</returns>
        /// <exception cref="NotFoundException">Không tìm thấy đối tượng</exception>
        /// Created by: nlnhat (13/07/2023)ks
        public virtual async Task<IEnumerable<TEntityDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            var result = _mapper.Map<IEnumerable<TEntityDto>>(entities);

            return result;
        }
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Dto đối tượng được tìm thấy</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task<TEntityDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);

            var result = _mapper.Map<TEntityDto>(entity);

            return result;
        }
        /// <summary>
        /// Lấy đối tượng theo danh sách id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>Đối tượng có id trong danh sách</returns>
        /// Created by: nlnhat (18/07/2023)
        public async Task<IEnumerable<TEntityDto>> GetManyAsync(IEnumerable<Guid> ids)
        {
            var entities = await _repository.GetManyAsync(ids);

            var result = _mapper.Map<IEnumerable<TEntityDto>>(entities);

            return result;
        }

        // Lọc
        public async Task<FilterResultModel<TEntityDto>> FilterAsync(
            string? keySearch, PagingModel? pagingModel, List<SortModel>? sortModels, List<FilterModel>? filterModels)
        {
            if (pagingModel != null && pagingModel.PageNumber < 1)
            {
                pagingModel.PageNumber = 1;
            }

            var accountId = await _authService.GetAccountId();

            var res = await _repository.FilterAsync(keySearch, pagingModel, sortModels, filterModels, accountId);

            var dtos = _mapper.Map<IEnumerable<TEntityDto>>(res.Data);

            var result = new FilterResultModel<TEntityDto>
            {
                TotalRecord = res.TotalRecord,
                AllRecord = res.AllRecord,
                PagingModel = res.PagingModel,
                Data = dtos
            };

            return result;
        }
        #endregion
    }
}
