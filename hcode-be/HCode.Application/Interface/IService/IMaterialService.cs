//using Microsoft.AspNetCore.Http;
//using HCode.Domain;

//namespace HCode.Application
//{
//    /// <summary>
//    /// Giao diện service nguyên vật liệu
//    /// </summary>
//    /// Created by: nlnhat (17/08/2023)
//    public interface IMaterialService : IBaseService<MaterialDto, Material>
//    {
//        /// <summary>
//        /// Lấy mã mới cho nguyên vật liệu
//        /// </summary>
//        /// <param name="materialName">Tên nguyên vật liệu</param>
//        /// <returns>Mã mới có dạng Prefix-Số</returns>
//        /// Created by: nlnhat (17/08/2023)
//        Task<string> GetNewCodeAsync(string materialName);
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
//        /// Xuất ra file excel
//        /// </summary>
//        /// <param name="keySearch">Từ khoá tìm kiếm</param>
//        /// <param name="sortModels">Các điều kiện sắp xếp</param>
//        /// <param name="filterModels">Các điều kiện lọc</param>
//        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
//        /// Created by: nlnhat (16/08/2023)
//        Task<MemoryStream> ExportToExcelAsync(string? keySearch, List<SortModel>? sortModels, List<FilterModel>? filterModels);
//        /// <summary>
//        /// Nhập nguyên vật liệu từ excel
//        /// </summary>
//        /// <param name="materialDtos">Danh sách dto tạo mới nguyên vật liệu</param>
//        /// <returns>Danh sách id tạo mới</returns>
//        /// Created by: nlnhat (13/09/2023)
//        Task<IEnumerable<Guid>> ImportFromExcelAsync(IEnumerable<MaterialDto> materialDtos);
//        /// <summary>
//        /// Đếm số lượng theo các năm
//        /// </summary>
//        /// <returns>Danh sách số lượng theo năm</returns>
//        /// Created by: nlnhat (08/09/2023)
//        Task<IEnumerable<CountByYearModel>> CountByYear();
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
//        /// <summary>
//        /// Map đơn vị chuyển đổi
//        /// </summary>
//        /// <param name="conversionUnitDtos">Danh sách dto đơn vị chuyển đổi</param>
//        /// <param name="materialId">Id nguyên vật liệu</param>
//        /// <returns>Danh sách đơn vị chuyển đổi tạo mới</returns>
//        /// Created by: nlnhat (17/08/2023)
//        List<ConversionUnit> MapCreateConversionUnits(List<ConversionUnitDto>? conversionUnitDtos, Guid materialId);
//        /// <summary>
//        /// Map dữ liệu từ file nhập khẩu
//        /// </summary>
//        /// <param name="file">File excel</param>
//        /// <returns></returns>
//        /// Created by: nlnhat (16/08/2023)
//        Task<IEnumerable<MaterialDto>> MapImportFileAsync(IFormFile file);
//        /// <summary>
//        /// Validate dữ liệu import sang dto hiển thị cho người dùng kết quả check dữ liệu import
//        /// </summary>
//        /// <param name="materialDtos">Danh sách dto import nguyên vật liệu</param>
//        /// <returns>Danh sách dto import nguyên vật liệu kèm dữ liệu hợp lệ hay không</returns>
//        /// Created by: nlnhat (12/09/2023)
//        Task<List<MaterialDto>> ValidateImportMaterialsAsync(List<MaterialDto> materialDtos);
//        /// <summary>
//        /// Validate danh sách đơn vị chuyển đổi
//        /// </summary>
//        /// <param name="conversionUnits">Danh sách dto đơn vị chuyển đổi</param>
//        /// <param name="unitId">Id của đơn vị tính chính</param>
//        Task ValidateConversionUnitsAsync(List<ConversionUnit> conversionUnits, Guid unitId);
//        /// <summary>
//        /// Validate danh sách đơn vị chuyển đổi cập nhật
//        /// </summary>
//        /// <param name="conversionUnits">Danh sách đơn vị chuyển đổi</param>
//        /// <param name="materialId">Id của nguyên vật liệu</param>
//        Task ValidateUpdateConversionUnitsAsync(List<ConversionUnit> conversionUnits, Guid materialId);
//    }
//}
