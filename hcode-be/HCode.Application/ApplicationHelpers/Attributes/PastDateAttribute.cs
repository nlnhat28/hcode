using Microsoft.Extensions.Localization;
using HCode.Domain;
using System.ComponentModel.DataAnnotations;

namespace HCode.Application
{
    /// <summary>
    /// Attribute validate ngày không lớn hơn hôm nay
    /// </summary>
    /// Created by: nlnhat (09/08/2023)
    public class PastDateAttribute : ValidationAttribute
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="resource">Resource lưu trữ thông báo</param>
        /// Created by: nlnhat (09/08/2023)
        public PastDateAttribute()
        {
            ErrorMessage = "Ngày được chọn không thể lớn hơn hôm nay";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check validate value
        /// </summary>
        /// <param name="value">Giá trị</param>
        /// <returns>True nếu nhỏ hơn hoặc bằng hôm nay, false nếu ngược lại</returns>
        /// Created by: nlnhat (09/08/2023)
        public override bool IsValid(object? value)
        {
            if (value == null) return true;

            if (DateTime.TryParse(value.ToString(), out DateTime dateInput))
            {
                var result = dateInput.CompareTo(DateTime.UtcNow);
                if (result <= 0) return true;
                return false;
            }
            else
            {
                return false;
            }
        } 
        #endregion
    }
}
