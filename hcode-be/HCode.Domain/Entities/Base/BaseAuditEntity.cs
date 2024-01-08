using System.ComponentModel.DataAnnotations;

namespace HCode.Domain
{
    /// <summary>
    /// Lớp thực thể cơ sở
    /// </summary>
    /// Created by: nlnhat (11/07/2023)
    public class BaseAuditEntity : IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Số thứ tự
        /// </summary>
        public int? RowNumber { get; set; }
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