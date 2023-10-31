namespace HCode.Application
{
    /// <summary>
    /// Giao diện service cho thực thể có mã
    /// </summary>
    /// <typeparam name="TEntity">Thực thể</typeparam>
    /// <typeparam name="TEntityDto">Dto thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public interface ICodeService<TEntityDto, TEntity>
    {
        /// <summary>
        /// Lấy đối tượng theo mã
        /// </summary>
        /// <param name="code">Mã đối tượng</param>
        /// <returns>Dto đối tượng</returns>
        /// Created by: nlnhat (17/08/2023)
        Task<TEntityDto> GetByCodeAsync(string code);
    }
}
