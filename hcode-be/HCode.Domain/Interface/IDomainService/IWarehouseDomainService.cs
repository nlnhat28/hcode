namespace HCode.Domain
{
    /// <summary>
    /// Giao diện validate nghiệp vụ nhà kho
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IWarehouseDomainService
    {
        /// <summary>
        /// Check trùng mã kho
        /// </summary>
        /// <param name="warehouseId">Id kho để check</param>
        /// <param name="warehouseCode">Mã kho để check</param>
        /// Created by: nlnhat (17/08/2023)
        Task CheckDuplicatedCodeAsync(Guid warehouseId, string warehouseCode);
        /// <summary>
        /// Check tồn tại nhà kho
        /// </summary>
        /// <param name="warehouseId">Id của kho</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistWarehouseAsync(Guid warehouseId);
    }
}
