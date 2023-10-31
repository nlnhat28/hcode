namespace HCode.Domain
{
    /// <summary>
    /// Mô hình lọc
    /// </summary>
    public class FilterModel
    {
        #region Properties
        /// <summary>
        /// Tên cột được lọc
        /// </summary>
        public string? ColumnName { get; set; }
        /// <summary>
        /// Kiểu so sánh
        /// </summary>
        public CompareType CompareType { get; set; }
        /// <summary>
        /// Logic so sánh
        /// </summary>
        public LogicType LogicType { get; set; }
        /// <summary>
        /// Giá trị lọc
        /// </summary>
        public string? FilterValue { get; set; }
        #endregion
    }
}
