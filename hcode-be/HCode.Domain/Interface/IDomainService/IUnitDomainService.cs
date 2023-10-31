namespace HCode.Domain
{
    /// <summary>
    /// Giao diện validate nghiệp vụ đơn vị
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public interface IUnitDomainService
    {
        /// <summary>
        /// Check trùng tên đơn vị tính
        /// </summary>
        /// <param name="unitId">Id đơn vị để check</param>
        /// <param name="unitName">Tên đơn vị để check</param>
        /// Created by: nlnhat (17/08/2023)
        Task CheckDuplicatedNameAsync(Guid unitId, string unitName);
        /// <summary>
        /// Check tồn tại đơn vị tính
        /// </summary>
        /// <param name="unitId">Id của đơn vị tính</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistUnitAsync(Guid unitId);
        /// <summary>
        /// Check tồn tại danh sách đơn vị tính
        /// </summary>
        /// <param name="unitIds">Danh sách id đơn vị tính</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistUnitsAsync(List<Guid> unitIds);
    }
}
