using HCode.Domain;
using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Lớp dto nhập nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public class MaterialImportDto
    {
        #region Properties
        /// <summary>
        /// Nã nguyên vật liệu
        /// </summary>
        /// </summary>
        [Required]
        [StringLength(20)]
        public string MaterialCode { get; set; }
        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        [Required]
        [StringLength(255)]
        public string MaterialName { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [Required]
        public string? UnitName { get; set; }
        /// <summary>
        /// Mã kho
        /// </summary>
        public string? WarehouseCode { get; set; }
        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public int? ExpiryTime { get; set; }
        /// <summary>
        /// Đơn vị thời gian hết hạn
        /// </summary>
        public TimeUnit? TimeUnit { get; set; }
        /// <summary>
        /// Số lượng tồn tối thiểu
        /// </summary>
        public decimal? MinimumInventory { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        [StringLength(255)]
        public string? Note { get; set; }
        /// <summary>
        /// Đơn vị chuyển đổi
        /// </summary>
        public ConversionUnitImportDto ConversionUnit { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo dto nguyên vật liệu từ các chuỗi
        /// </summary>
        /// <param name="materialCode">Mã nguyên vật liệu</param>
        /// <param name="materialName">Tên nguyên vật liệu</param>
        /// <param name="minimumInventory">Số lượng tồn tối thiểu</param>
        /// <param name="unitName">Tên đơn vị tính</param>
        /// <param name="warehouseCode">Mã kho</param>
        /// <param name="expiryTime">Hạn sử dụng</param>
        /// <param name="timeUnit">Đơn vị thời gian</param>
        /// <param name="note">Ghi chú</param>
        /// <param name="conversionUnitDto">Dto đơn vị chuyển đổi</param>
        /// Created by: nlnhat (11/09/2023)
        public MaterialImportDto(string materialCode, string materialName, string minimumInventory, string unitName,
            string warehouseCode, string expiryTime, string timeUnit, string note, ConversionUnitImportDto conversionUnitImportDto)
        {
            MaterialCode = materialCode;
            MaterialName = materialName;

            if (decimal.TryParse(minimumInventory, out var _minimumInventory))
            {
                MinimumInventory = _minimumInventory;
            }

            UnitName = unitName;
            WarehouseCode = warehouseCode;

            if (int.TryParse(expiryTime, out var _expireTime))
            {
                ExpiryTime = _expireTime;
            }

            TimeUnit = timeUnit switch
            {
                "Ngày" => Domain.TimeUnit.Day,
                "Tháng" => Domain.TimeUnit.Month,
                "Năm" => Domain.TimeUnit.Year,
                _ => null,
            };

            Note = note;

            ConversionUnit = conversionUnitImportDto;
        }
        #endregion
    }
}
