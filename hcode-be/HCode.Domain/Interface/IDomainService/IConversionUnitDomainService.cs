namespace HCode.Domain
{
    /// <summary>
    /// Giao diện validate nghiệp vụ đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public interface IConversionUnitDomainService
    {
        /// <summary>
        /// Check tồn tại danh sách đơn vị chuyển đổi theo nguyên vật liệu
        /// </summary>
        /// <param name="conversionUnitIds">Danh sách id đơn vị chuyển đổi</param>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistConversionUnitsAsync(List<Guid> conversionUnitIds, Guid materialId);
        /// <summary>
        /// Check tồn tại danh sách đơn vị muốn chuyển đổi
        /// </summary>
        /// <param name="destinationUnitIds">Danh sách id đơn vị muốn chuyển đổi</param>
        /// Created by: nlnhat (30/08/2023)
        Task CheckExistDestinationUnitsAsync(List<Guid> destinationUnitIds);
    }
}
