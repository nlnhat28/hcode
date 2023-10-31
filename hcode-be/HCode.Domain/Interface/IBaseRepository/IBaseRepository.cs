namespace HCode.Domain
{
    /// <summary>
    /// Giao diện repository cơ sở
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public interface IBaseRepository<TEntity> : IReadOnlyRepository<TEntity>
    {
        /// <summary>
        /// Tạo mới 1 đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng muốn tạo</param>
        /// <returns>Id của bản ghi mới</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<Guid> InsertAsync(TEntity entity);
        /// <summary>
        /// Tạo mới nhiều đối đối tượng
        /// </summary>
        /// <param name="entities">Các đối tượng muốn tạo</param>
        /// <returns>Id của các bản ghi mới</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<int> InsertManyAsync(IEnumerable<TEntity>? entities);
        /// <summary>
        /// Cập nhật 1 đối tượng
        /// </summary>
        /// <param name="entity">Thông tin mới</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<int> UpdateAsync(TEntity entity);
        /// <summary>
        /// Cập nhật nhiều đối tượng
        /// </summary>
        /// <param name="entities">Các đối tượng muốn cập nhật</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<int> UpdateManyAsync(IEnumerable<TEntity>? entities);
        /// <summary>
        /// Xoá 1 đối tượng
        /// </summary>
        /// <param name="id">Id đối tượng muốn xoá</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<int> DeleteAsync(Guid id);
        /// <summary>
        /// Xoá nhiều đối tượng
        /// </summary>
        /// <param name="ids">Danh sách id đối tượng muốn xoá</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<int> DeleteManyAsync(IEnumerable<Guid>? ids);
    }
}
