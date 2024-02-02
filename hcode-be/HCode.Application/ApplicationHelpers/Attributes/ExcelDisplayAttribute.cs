using HCode.Domain;
using OfficeOpenXml.Style;

namespace HCode.Application
{
    /// <summary>
    /// Attribute tuỳ chỉnh cột hiển thị trong excel 
    /// </summary>
    /// Created by: nlnhat (23/07/2023)
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelDisplayAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Chữ cái định vị cột
        /// </summary>
        public string ColumnLetter { get; set; } = "A";
        /// <summary>
        /// Tên cột
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Căn chỉnh nội dung cột theo chiều ngang
        /// </summary>
        public ExcelHorizontalAlignment HorizontalAlignment { get; set; }
        /// <summary>
        /// Định dạng dữ liệu cột
        /// </summary>
        public string? NumberFormat { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm khởi tạo ExcelDisplayAttribute
        /// </summary>
        /// <param name="columnLetter">Chữ cái định vị cột</param>
        /// <param name="columnName">Tên cột</param>
        /// <param name="horizontalAlignment">Căn chỉnh theo chiều ngang</param>
        /// <param name="numberFormat">Format dữ liệu</param>
        /// Created by: nlnhat (23/07/2023)
        public ExcelDisplayAttribute(
            string columnLetter,
            string columnName,
            string? horizontalAlignment = HorizontalAlign.Left,
            string? numberFormat = null)
        {
            ColumnLetter = columnLetter;
            ColumnName = columnName;
            NumberFormat = numberFormat;

            switch (horizontalAlignment)
            {
                case HorizontalAlign.Left:
                    {
                        HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        break;
                    }
                case HorizontalAlign.Center:
                    {
                        HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    }
                case HorizontalAlign.Right:
                    {
                        HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        break;
                    }
                default:
                    {
                        HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        break;
                    }
            }
        }
        #endregion
    }
}
