using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Lớp Dto cơ sở
    /// </summary>
    /// Created by: nlnhat (11/07/2023)
    public class BaseDto
    {
        #region Properties
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; } 
        /// <summary>
        /// Người tạo
        /// </summary>
        [StringLength(255)]
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>  
        [StringLength(255)]
        public string? ModifiedBy { get; set; }
        #endregion
    }
}