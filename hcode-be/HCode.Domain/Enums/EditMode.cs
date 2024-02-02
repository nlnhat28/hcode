namespace HCode.Domain
{
    /// <summary>
    /// Enum thao tác với bản ghi
    /// </summary>
    /// Created by: nlnhat (12/07/2023)
    public enum EditMode
    {
        /// <summary>
        /// Không sửa gì
        /// </summary>
        NoEdit = 0,
        /// <summary>
        /// Thêm
        /// </summary>
        Create = 1,
        /// <summary>
        /// Sửa
        /// </summary>
        Update = 2,
        /// <summary>
        /// Xoá
        /// </summary>
        Delete = 3,
    }
}