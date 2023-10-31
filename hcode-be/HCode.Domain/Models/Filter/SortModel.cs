using System.ComponentModel.DataAnnotations;

namespace HCode.Domain
{
    /// <summary>
    /// Mô hình sắp xếp
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public class SortModel
    {
        #region Properties
        /// <summary>
        /// Tên cột sắp xếp
        /// </summary>
        [Required]
        public string ColumnName { get; set; }
        /// <summary>
        /// Kiểu sắp xếp
        /// </summary>
        public SortType SortType { get; set; }
        #endregion
    }
}
