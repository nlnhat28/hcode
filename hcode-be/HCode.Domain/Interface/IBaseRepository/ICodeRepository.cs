namespace HCode.Domain
{
    /// <summary>
    /// Giao diện repository cho những thực thể có mã
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public interface ICodeRepository<TEntity>
    {
        /// <summary>
        /// Lấy đối tượng theo mã
        /// </summary>
        /// <param name="code">Mã của đối tượng</param>
        /// <returns>Đối tượng được tìm thấy</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<TEntity> GetByCodeAsync(string code);
        /// <summary>
        /// Lấy nhiều đối tượng theo mã
        /// </summary>
        /// <param name="codes">Danh sách mã của đối tượng</param>
        /// <returns>Danh sách đối tượng được tìm thấy</returns>
        /// Created by: nlnhat (15/08/2023)
        Task<IEnumerable<TEntity>> GetManyByCodeAsync(List<string>? codes);
    }
}
