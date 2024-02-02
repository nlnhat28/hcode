namespace HCode.Application
{
    /// <summary>
    /// Dto cơ sở của đối tượng khi lọc
    /// </summary>
    /// <typeparam name="TEntityDto">Dto thực thể</typeparam>
    /// Created by: nlnhat (18/07/2023)
    public class BaseFilterDto<TEntityDto>
    {
        #region Properties
        /// <summary>
        /// Số bản ghi được lọc
        /// </summary>
        public int TotalRecord { get; set; } 
        /// <summary>
        /// Tổng số bản ghi trong database
        /// </summary>
        public int AllRecord { get; set; }
        /// <summary>
        /// Số trang hiện tại
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Danh sách dto đối tượng thoả mãn điều kiện lọc
        /// </summary>
        public IEnumerable<TEntityDto>? Data { get; set; }
        #endregion
    }
}
