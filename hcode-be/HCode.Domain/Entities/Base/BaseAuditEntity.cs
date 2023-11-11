using System.ComponentModel.DataAnnotations;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp thực thể cơ sở
    /// </summary>
    /// Created by: nlnhat (11/07/2023)
    public class BaseAuditEntity
    {
        #region Properties
        public int? RowIndex { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
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