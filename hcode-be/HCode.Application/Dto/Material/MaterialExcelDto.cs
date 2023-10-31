using HCode.Domain;
using OfficeOpenXml.Attributes;
using System.ComponentModel;

namespace HCode.Application
{
    /// <summary>
    /// Lớp excel dto nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    [DisplayName("Thông tin chung")]
    public class MaterialExcelDto
    {
        /// <summary>
        /// Số thứ tự
        /// </summary>
        [ExcelDisplay("A", "Số thứ tự")]
        public int Index { get; set; }
        /// <summary>
        /// Nã nguyên vật liệu
        /// </summary>
        [ExcelDisplay("B", "Mã")]
        public string MaterialCode { get; set; }
        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        [ExcelDisplay("C", "Tên")]
        public string MaterialName { get; set; }
        /// <summary>
        /// Số lượng tồn tối thiểu
        /// </summary>
        [ExcelDisplay("D", "Số lượng tồn", HorizontalAlign.Right, "#,##0.00")]
        public decimal? MinimumInventory { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [ExcelDisplay("E", "Đơn vị tính")]
        public string? UnitName { get; set; }
        /// <summary>
        /// Tên kho
        /// </summary>
        [ExcelDisplay("F", "Kho ngầm định")]
        public string? WarehouseName { get; set; }
        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        [ExcelDisplay("G", "Hạn sử dụng", HorizontalAlign.Right)]
        public int? ExpiryTime { get; set; }
        /// <summary>
        /// Tên đơn vị thời gian
        /// </summary>
        [ExcelDisplay("H", "Đơn vị thời gian")]
        public string? TimeUnitName { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        [ExcelDisplay("I", "Ghi chú")]
        public string? Note { get; set; }
        /// <summary>
        /// Ngừng theo dõi hay không
        /// </summary>
        [ExcelDisplay("J", "Ngừng theo dõi")]
        public bool? IsUnfollow { get; set; }
        /// <summary>
        /// Danh sách đơn vị chuyển đổi
        /// </summary>
        [EpplusIgnore]
        public List<ConversionUnitExcelDto>? ConversionUnits { get; set; }
        /// <summary>
        /// Cột bắt đầu
        /// </summary>
        [EpplusIgnore]
        public static string FirstColumn { get; set; } = "A";
        /// <summary>
        /// Cột kết thúc
        /// </summary>
        [EpplusIgnore]
        public static string LastColumn { get; set; } = "J";
    }
}
