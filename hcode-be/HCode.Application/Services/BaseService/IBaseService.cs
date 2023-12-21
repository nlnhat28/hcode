﻿using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Giao diện service cơ sở
    /// </summary>
    /// <typeparam name="TEntityDto">Dto đối tượng</typeparam>
    /// <typeparam name="TEntity">Đối tượng</typeparam>
    /// Created by: nlnhat (17/08/2023)
    public interface IBaseService<TEntityDto, TEntity> : IReadOnlyService<TEntityDto, TEntity> where TEntityDto : BaseDto where TEntity : BaseEntity
    {
        /// <summary>
        /// Tạo mới đối tượng
        /// </summary>
        /// <param name="entityDto">Dto tạo mới đối tượng</param>
        /// <returns>Id của bản ghi mới</returns>
        /// Created by: nlnhat (17/08/2023)
        Task CreateAsync(TEntityDto entityDto, ServerResponse res);
        /// <summary>
        /// Sửa đối tượng
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <param name="entityDto">Dto cập nhật đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (17/08/2023)
        Task UpdateAsync(Guid id, TEntityDto entityDto, ServerResponse res);
        /// <summary>
        /// Xoá đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (17/08/2023)
        Task<int> DeleteAsync(Guid id);
        /// <summary>
        /// Xoá nhiều đối tượng
        /// </summary>
        /// <param name="ids">List id đối tượng muốn xoá</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (17/07/2023)
        Task<int> DeleteManyAsync(IEnumerable<Guid> ids);
        /// <summary>
        /// Xoá hết đối tượng
        /// </summary>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (17/07/2023)
        Task<int> DeleteAllAsync();
        /// <summary>
        /// Validate đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng để validate</param>
        /// Created by: nlnhat (17/08/2023)
        Task ValidateAsync(TEntity entity, ServerResponse res);
        /// <summary>
        /// Map dto tạo mới sang thực thể
        /// </summary>
        /// <param name="entityDto">Dto tạo mới</param>
        /// <returns>Thực thể</returns>
        /// Created by: nlnhat (18/07/2023)
        TEntity MapCreateDtoToEntity(TEntityDto entityDto);
        /// <summary>
        /// Map dto cập nhật sang thực thể
        /// </summary>
        /// <param name="entityDto">Dto cập nhật</param>
        /// <returns>Thực thể</returns>
        /// Created by: nlnhat (18/07/2023)
        TEntity MapUpdateDtoToEntity(TEntityDto entityDto);
    }
}
