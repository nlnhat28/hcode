//namespace HCode.Domain
//{
//    /// <summary>
//    /// Giao diện repository nguyên vật liệu
//    /// </summary>
//    /// Created by: nlnhat (15/08/2023)
//    public interface IMaterialRepository : IBaseRepository<Material>, ICodeRepository<Material>
//    {
//        /// <summary>
//        /// Lấy nguyên vật liệu theo id
//        /// </summary>
//        /// <param name="id">Mã nguyên vật liệu</param>
//        /// <returns>Mô hình nguyên vật liệu</returns>
//        /// Created by: nlnhat (15/08/2023)
//        new Task<MaterialModel> GetAsync(Guid id);
//        /// <summary>
//        /// Lấy mã nguyên vật liệu lớn nhất hiện tại theo tiền tố
//        /// </summary>
//        /// <param name="prefix">Tiền tố của mã</param>
//        /// <returns>Mã nguyên vật liệu lớn nhất</returns>
//        /// Created by: nlnhat (15/08/2023)
//        Task<int> GetMaxCodeAsync(string prefix);
//        /// <summary>
//        /// Lọc nâng cao nguyên vật liệu (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
//        /// </summary>
//        /// <param name="keySearch">Từ khoá tìm kiếm</param>
//        /// <param name="pagingModel">Các thuộc tính phân trang</param>
//        /// <param name="sortModels">Các điều kiện sắp xếp</param>
//        /// <param name="filterModels">Các điều kiện lọc</param>
//        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
//        /// Created by: nlnhat (16/08/2023)
//        Task<FilterResultModel<Material>> FilterAsync(string? keySearch, PagingModel? pagingModel, 
//            List<SortModel>? sortModels, List<FilterModel>? filterModels);
//        /// <summary>
//        /// Lọc nguyên vật liệu để export
//        /// </summary>
//        /// <param name="keySearch">Từ khoá tìm kiếm</param>
//        /// <param name="sortModels">Các điều kiện sắp xếp</param>
//        /// <param name="filterModels">Các điều kiện lọc</param>
//        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
//        /// Created by: nlnhat (16/08/2023)
//        Task<IEnumerable<MaterialModel>> ExportAsync(string? keySearch, List<SortModel>? sortModels, List<FilterModel>? filterModels);
//        /// <summary>
//        /// Đếm số lượng theo các kho
//        /// </summary>
//        /// <returns>Danh sách số lượng theo kho</returns>
//        /// Created by: nlnhat (08/09/2023)
//        Task<IEnumerable<CountByWarehouseModel>> CountByWarehouse();
//        /// <summary>
//        /// Đếm số lượng theo trạng thái theo dõi
//        /// </summary>
//        /// <returns>Số lượng theo trạng thái</returns>
//        /// Created by: nlnhat (08/09/2023)
//        Task<CountByFollowModel> CountByFollow();
//    }
//}
