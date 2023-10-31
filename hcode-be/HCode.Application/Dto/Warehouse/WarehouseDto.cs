using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Dto tạo mới kho
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class WarehouseDto : BaseDto
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Required]
        public Guid WarehouseId { get; set; }
        /// <summary>
        /// Mã nhà kho
        /// </summary>
        [Required]
        [StringLength(20)]
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Tên nhà kho
        /// </summary>
        [Required]
        [StringLength(255)]
        public string WarehouseName { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        [StringLength(255)]
        public string? Address { get; set; }
    }
}


