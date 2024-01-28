
namespace HCode.Domain
{
    /// <summary>
    /// Attribute không cập nhật
    /// </summary>
    /// Created by: nlnhat (23/07/2023)
    [AttributeUsage(AttributeTargets.Property)]
    public class ScriptAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Không update
        /// </summary>
        public bool IsNotUpdate { get; set; }
        /// <summary>
        /// Where khi update
        /// </summary>
        public bool IsWhereUpdate { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="columns">Tên cột có thể tìm kiếm</param>
        public ScriptAttribute(bool isNotUpdate = false, bool isWhereUpdate = false)
        {
            IsNotUpdate = isNotUpdate;
            IsWhereUpdate = isWhereUpdate;
        }
        #endregion
    }
}
