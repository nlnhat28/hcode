using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Dto chứa data để lọc
    /// </summary>
    public class FilterRequestDto
    {
        /// <summary>
        /// Từ khoá tìm kiếm
        /// </summary>
        public string? KeySearch { get; set; }
        /// <summary>
        /// Mô hình phân trang
        /// </summary>
        public PagingModel? PagingModel { get; set; }
        /// <summary>
        /// Các điều kiến sắp xếp
        /// </summary>
        public List<SortModel>? SortModels { get; set; }
        /// <summary>
        /// Các điều kiện lọc theo cột
        /// </summary>
        public List<FilterModel>? FilterModels { get; set; }
    }
}
