namespace HCode.Domain
{
    /// <summary>
    /// Giao diện repository nhật ký nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (08/09/2023)
    public interface IMaterialAuditRepository : IBaseRepository<MaterialAudit>
    {
        /// <summary>
        /// Đếm số lượng theo các năm
        /// </summary>
        /// <returns>Danh sách số lượng theo năm</returns>
        /// Created by: nlnhat (08/09/2023)
        Task<IEnumerable<CountByYearModel>> CountByYear();
    }
}