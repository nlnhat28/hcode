using HCode.Domain;
using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Dto đơn vị tính
    /// </summary>
    /// Created by: nlnhat (16/08/2023)
    public class UnitDto : BaseDto
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Required]
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        [Required]
        [StringLength(255)]
        public string UnitName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

    }
}
