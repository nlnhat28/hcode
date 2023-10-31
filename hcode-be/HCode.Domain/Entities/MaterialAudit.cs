namespace HCode.Domain
{
    /// <summary>
    /// Lớp ghi log thêm/xoá nguyên vật liệu
    /// </summary>
    /// Created by: nlnhat (09/09/2023)
    public class MaterialAudit
    {
        #region Properties
        /// <summary>
        /// Khoá chính 
        /// </summary>
        public Guid MaterialAuditId { get; set; }
        /// <summary>
        /// Id nguyên vật liệu
        /// </summary>
        public Guid MaterialId { get; set; }
        /// <summary>
        /// Enum chế độ chỉnh sửa
        /// </summary>
        public EditMode EditMode { get; set; }
        /// <summary>
        /// Thời gian chỉnh sửa
        /// </summary>
        public DateTime TimeStamp { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo MaterialAudit
        /// </summary>
        /// <param name="materialAuditId">Khoá chính</param>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <param name="editMode">Enum chế độ chỉnh sửa</param>
        /// <param name="timeStamp">Thời gian chỉnh sửa</param>
        public MaterialAudit(Guid materialAuditId, Guid materialId, EditMode editMode, DateTime timeStamp)
        {
            MaterialAuditId = materialAuditId;
            MaterialId = materialId;
            EditMode = editMode;
            TimeStamp = timeStamp;
        }
        #endregion
    }
}
