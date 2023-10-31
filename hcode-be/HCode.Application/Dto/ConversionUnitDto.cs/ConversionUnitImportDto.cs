using HCode.Domain;
using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Lớp dto nhập khẩu đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (15/08/2023)
    public class ConversionUnitImportDto
    {
        #region Properties
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
        /// Thứ tự trong bảng trên giao diện
        /// </summary>
        public int RowIndex { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo dto đơn vị chuyển đổi từ các chuỗi
        /// </summary>
        /// <param name="destinationUnitName">Tên đơn vị muốn chuyển đổi</param>
        /// <param name="rate">Tỷ lệ chuyển đổi</param>
        /// <param name="operatorName">Tên phép tính</param>
        /// Created by: nlnhat (11/09/2023)
        public ConversionUnitImportDto(string destinationUnitName, string rate, string operatorName)
        {
            DestinationUnitName = destinationUnitName;

            if (decimal.TryParse(rate, out var _rate))
            {
                Rate = _rate == 0 ? 1 : _rate;
            }

            Operator = operatorName switch
            {
                "*" => Operator.Multiple,
                "/" => Operator.Divide,
                _ => Operator.Multiple
            };
        }
        #endregion
    }
}
