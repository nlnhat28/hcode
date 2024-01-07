using AutoMapper;
using Microsoft.Extensions.Localization;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace HCode.Application
{
    /// <summary>
    /// Service cơ sở đầy đủ
    /// </summary>
    /// <typeparam name="TEntityDto">Dto thực thể</typeparam>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public abstract class BaseService<TEntityDto, TEntity>
                        : ReadOnlyService<TEntityDto, TEntity>, IBaseService<TEntityDto, TEntity> 
                        where TEntityDto : BaseDto, IHasEntityId where TEntity : BaseEntity, IHasEntityId
    {
        #region Fields
        /// <summary>
        /// Repository tương tác đầy đủ với database
        /// </summary>
        /// Created by: nlnhat (13/07/2023)ks
        protected new readonly IBaseRepository<TEntity> _repository;
        /// <summary>
        /// Unit of work
        /// </summary>
        /// Created by: nlnhat (13/07/2023)ks
        protected readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service đầy đủ
        /// </summary>
        /// <param name="repository">Repository tương tác đầy đủ với database</param>
        /// <param name="resource">Resource lưu thông báo</param>
        /// <param name="mapper">Mapper đối tượng</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (18/07/2023)
        public BaseService(
            IBaseRepository<TEntity> repository, 
            IStringLocalizer<Resource> resource, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IAuthService authService)
            : base(repository, resource, mapper, authService) 
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validate thực thể trước khi đưa vào database
        /// </summary>
        /// <param name="entity">Thực thể</param>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task ValidateAsync(TEntity entity, ServerResponse res)
        {
            await Task.Yield();
        }
        /// <summary>
        /// Map dto tạo mới sang thực thể
        /// </summary>
        /// <param name="entityDto">Dto tạo mới</param>
        /// <returns>Thực thể</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual TEntity MapCreateDtoToEntity(TEntityDto entityDto)
        {
            entityDto.Id = Guid.NewGuid();
            entityDto.CreatedTime ??= DateTime.UtcNow;
            entityDto.CreatedBy = _authService.GetAccountIdToString();

            var result = _mapper.Map<TEntity>(entityDto);
            return result;
        }
        /// <summary>
        /// Map dto cập nhật sang thực thể
        /// </summary>
        /// <param name="entityDto">Dto cập nhật</param>
        /// <returns>Thực thể</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual TEntity MapUpdateDtoToEntity(TEntityDto entityDto)
        {
            entityDto.ModifiedTime = DateTime.UtcNow;
            entityDto.ModifiedBy = _authService.GetAccountIdToString();

            var result = _mapper.Map<TEntity>(entityDto);
            return result;
        }
        /// <summary>
        /// Tạo mới đối tượng 
        /// </summary>
        /// <param name="entityDto">Dto tạo mới đối tượng</param>
        /// <returns>Id của bản ghi mới</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task CreateAsync(TEntityDto entityDto, ServerResponse res)
        {
            var entity = MapCreateDtoToEntity(entityDto);

            await ValidateAsync(entity, res);

            if (res.Success)
            {
                res.Data = await _repository.InsertAsync(entity);
            }
        }
        /// <summary>
        /// Cập nhật đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <param name="entityDto">Dto cập nhật đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task UpdateAsync(Guid id, TEntityDto entityDto, ServerResponse res)
        {
            _ = await _repository.GetAsync(id);

            var entity = MapUpdateDtoToEntity(entityDto);

            await ValidateAsync(entity, res);

            if (res.Success)
            {
                res.Data = await _repository.UpdateAsync(entity);
            }

        }
        /// <summary>
        /// Xoá đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var result = await _repository.DeleteAsync(id);
            return result;
        }
        /// <summary>
        /// Xoá nhiều đối tượng
        /// </summary>
        /// <param name="ids">List id muốn xoá</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task<int> DeleteManyAsync(IEnumerable<Guid> ids)
        {
            var result = await _repository.DeleteManyAsync(ids);
            return result;
        }
        /// <summary>
        /// Xoá hết đối tượng
        /// </summary>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task<int> DeleteAllAsync()
        {
            var result = await _repository.DeleteAllAsync();
            return result;
        }
        #endregion
    }
}