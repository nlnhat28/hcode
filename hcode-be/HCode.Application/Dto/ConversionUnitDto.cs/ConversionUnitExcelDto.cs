using HCode.Domain;
using OfficeOpenXml.Attributes;
using System.ComponentModel;

namespace HCode.Application
{
    /// <summary>
    /// Lớp dto excel đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (15/08/2023)
    [DisplayName("Đơn vị chuyển đổi")]
    public class ConversionUnitExcelDto
    {
        /// <summary>
        /// Tên đơn vị muốn chuyển
        /// </summary>
        [ExcelDisplay("K", "Tên đơn vị")]
        public string? DestinationUnitName { get; set; }
        /// <summary>
        /// Tỉ lệ chuyển đổi
        /// </summary>
        [ExcelDisplay("L", "Tỷ lệ chuyển đổi", HorizontalAlign.Right, "#,##0.00")]
        public decimal Rate { get; set; }
        /// <summary>
        /// Tên phép toán chuyển đổi
        /// </summary>
        [ExcelDisplay("M", "Phép tính")]
        public string? OperatorName { get; set; }
        /// <summary>
        /// Thao tác chỉnh sửa
        /// </summary>
        [ExcelDisplay("N", "Mô tả")]
        public string? Description { get; set; }
        /// <summary>
        /// Tên đơn vị tính của nguyên vât liệu
        /// </summary>
        [EpplusIgnore]
        public string? UnitName { get; set; }
        /// <summary>
        /// Thứ tự trong bảng trên giao diện
        /// </summary>
        [EpplusIgnore]
        public int RowIndex { get; set; }
        /// <summary>
        /// Cột bắt đầu
        /// </summary>
        [EpplusIgnore]
        public static string FirstColumn { get; set; } = "K";
        /// <summary>
        /// Cột kết thúc
        /// </summary>
        [EpplusIgnore]
        public static string LastColumn { get; set; } = "N";
    }
}
