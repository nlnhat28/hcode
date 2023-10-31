namespace HCode.Domain
{
    /// <summary>
    /// Giao diện repository đơn vị tính
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public interface IUnitRepository : IBaseRepository<Unit>
    {
        /// <summary>
        /// Lấy đơn vị tính theo tên
        /// </summary>
        /// <param name="unitName">Tên của đơn vị tính</param>
        /// <returns>Đơn vị tính có tên trùng với param</returns>
        /// Created by: nlnhat (16/08/2023)
        Task<Unit> GetByNameAsync(string unitName);
        /// <summary>
        /// Lấy nhiều đơn vị tính theo tên
        /// </summary>
        /// <param name="names">Danh sách tên đơn vị tính</param>
        /// <returns>Danh sách đơn vị có tên tương ứng</returns>
        Task<IEnumerable<Unit>> GetManyByNameAsync(List<string> names);
        /// <summary>
        /// Lấy tất cả id của đơn vị tính
        /// </summary>
        /// <returns>Danh sách tất cả id đơn vị tính</returns>
        Task<IEnumerable<Guid>> GetAllIdAsync();
    }
}