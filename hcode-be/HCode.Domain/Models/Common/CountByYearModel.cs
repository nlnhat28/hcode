namespace HCode.Domain
{
    /// <summary>
    /// Đếm số lượng theo năm
    /// </summary>
    /// Created by: nlnhat (08/09/2023)
    public class CountByYearModel
    {
        /// <summary>
        /// Năm
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Số lượng thêm mới
        /// </summary>
        public int CreateCount { get; set; }
        /// <summary>
        /// Số lượng xoá
        /// </summary>
        public int DeleteCount { get; set; }
    }
}
