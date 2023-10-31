using HCode.Domain;
using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Lớp dto đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (15/08/2023)
    public class ConversionUnitDto
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Required]
        public Guid ConversionUnitId { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng nguyên vật liệu
        /// </summary>
        [Required]
        public Guid MaterialId { get; set; }
        /// <summary>
        /// Khoá ngoại tham chiếu bảng đơn vị muốn chuyển
        /// </summary>
        [Required]
        public Guid DestinationUnitId { get; set; }
        /// <summary>
        /// Tên đơn vị muốn chuyển
        /// </summary>
        public string? DestinationUnitName { get; set; }
        /// <summary>
        /// Tỉ lệ chuyển đổi
        /// </summary>
        [Required]
        [Range(0, 9999999999999999.99)]
        public decimal Rate { get; set; }
        /// <summary>
        /// Phép toán chuyển đổi
        /// </summary>
        [Required]
        public Operator Operator { get; set; }
        /// <summary>
        /// Tên phép toán chuyển đổi
        /// </summary>
        public string? OperatorName { get; set; }
        /// <summary>
        /// Thao tác chỉnh sửa
        /// </summary>
        public EditMode? EditMode { get; set; }
        /// <summary>
        /// Thứ tự trong bảng trên giao diện
        /// </summary>
        public int RowIndex { get; set; }
        #endregion
    }
}
