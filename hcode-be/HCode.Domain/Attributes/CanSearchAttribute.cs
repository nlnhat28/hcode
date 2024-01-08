
namespace HCode.Domain
{
    /// <summary>
    /// Attribute có thể search
    /// </summary>
    /// Created by: nlnhat (23/07/2023)
    [AttributeUsage(AttributeTargets.Class)]
    public class CanSearchAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Tên column để search
        /// </summary>
        public List<string>? Columns { get; set; }
        #endregion

        #region Constructors
        public CanSearchAttribute() {}
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="columns">Tên cột có thể tìm kiếm</param>
        public CanSearchAttribute(string? columns)
        {
            if (!string.IsNullOrEmpty(columns))
            {
                Columns = columns.Split(",").Select(name => name.Trim()).ToList();
            }
        } 
        #endregion
    }
}
