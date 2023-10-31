using AutoMapper;
using Microsoft.Extensions.Localization;
using HCode.Domain;

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
        public BaseService(IBaseRepository<TEntity> repository, IStringLocalizer<Resource> resource, IMapper mapper, IUnitOfWork unitOfWork)
            : base(repository, resource, mapper) 
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
        public virtual async Task ValidateAsync(TEntity entity)
        {
            await Task.Yield();
        }
        /// <summary>
        /// Map dto tạo mới sang thực thể
        /// </summary>
        /// <param name="entityDto">Dto tạo mới</param>
        /// <returns>Thực thể</returns>
        /// Created by: nlnhat (18/07/2023)
        public abstract TEntity MapCreateDtoToEntity(TEntityDto entityDto);
        /// <summary>
        /// Map dto cập nhật sang thực thể
        /// </summary>
        /// <param name="entityDto">Dto cập nhật</param>
        /// <returns>Thực thể</returns>
        /// Created by: nlnhat (18/07/2023)
        public abstract TEntity MapUpdateDtoToEntity(TEntityDto entityDto);
        /// <summary>
        /// Tạo mới đối tượng 
        /// </summary>
        /// <param name="entityDto">Dto tạo mới đối tượng</param>
        /// <returns>Id của bản ghi mới</returns>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task<Guid> CreateAsync(TEntityDto entityDto)
        {
            var entity = MapCreateDtoToEntity(entityDto);

            await ValidateAsync(entity);

            var result = await _repository.InsertAsync(entity);
            return result;
        }
        /// <summary>
        /// Cập nhật đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <param name="entityDto">Dto cập nhật đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// <exception cref="NotFoundException">Không tìm thấy bản ghi</exception>
        /// Created by: nlnhat (18/07/2023)
        public virtual async Task<int> UpdateAsync(Guid id, TEntityDto entityDto)
        {
            _ = await _repository.GetAsync(id) ??
                throw new NotFoundException(data: new ExceptionData("Id", id.ToString()));

            var entity = MapUpdateDtoToEntity(entityDto);

            await ValidateAsync(entity);

            var result = await _repository.UpdateAsync(entity);
            return result;
        }
        /// <summary>
        /// Xoá đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// <exception cref="NotFoundException">Không tìm thấy bản ghi</exception>
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
        #endregion
    }
}