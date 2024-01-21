using System.ComponentModel.DataAnnotations;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp thực thể cơ sở
    /// </summary>
    /// Created by: nlnhat (11/07/2023)
    public class BaseEntity
    {
        #region Properties
        /// <summary>
        /// Id
        /// </summary>
        //public Guid Id { get; set; }
        /// <summary>
        /// Số thứ tự
        /// </summary>
        [NotMapped]
        public int? RowNumber { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        [NoUpdate]
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        [NoUpdate]
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime? ModifiedTime { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>  
        public string? ModifiedBy { get; set; }
        #endregion
    }
}