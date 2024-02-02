
namespace HCode.Domain
{
    /// <summary>
    /// Attribute không cập nhật
    /// </summary>
    /// Created by: nlnhat (23/07/2023)
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ScriptAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Không update
        /// </summary>
        public bool IsNotUpdate { get; set; }
        /// <summary>
        /// Bỏ qua các trường audit
        /// </summary>
        public bool IsIgnoreAudit { get; set; }
        /// <summary>
        /// Where khi update
        /// </summary>
        public bool IsWhereUpdate { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo
        /// </summary>
        public ScriptAttribute(bool isNotUpdate = false, bool isWhereUpdate = false, bool isIgnoreAudit = false)
        {
            IsNotUpdate = isNotUpdate;
            IsWhereUpdate = isWhereUpdate;
            IsIgnoreAudit = isIgnoreAudit;
        }
        #endregion
    }
}
