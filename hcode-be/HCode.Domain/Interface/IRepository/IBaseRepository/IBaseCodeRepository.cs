namespace HCode.Domain
{
    /// <summary>
    /// Giao diện repository dành cho thực thể có code
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public interface IBaseCodeRepository<TEntity> : IBaseRepository<TEntity>
    {
        /// <summary>
        /// Lấy đối tượng theo mã
        /// </summary>
        /// <param name="code">Mã của đối tượng</param>
        /// <returns>Đối tượng được tìm thấy</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<TEntity?> GetByCodeAsync(string code);
        /// <summary>
        /// Kiểm tra mã tồn tại không
        /// </summary>
        /// <param name="code">Mã của đối tượng</param>
        /// <param name="id">Mã của đối tượng</param>
        /// <returns>True nếu trùng mã nhưng không trùng id</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<bool> CheckExistedCodeAsync(string code, Guid id);
        /// <summary>
        /// Lấy nhiều đối tượng theo mã
        /// </summary>
        /// <param name="codes">Danh sách mã của đối tượng</param>
        /// <returns>Danh sách đối tượng được tìm thấy</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<IEnumerable<TEntity>> GetManyByCodeAsync(List<string>? codes);
    }
}
