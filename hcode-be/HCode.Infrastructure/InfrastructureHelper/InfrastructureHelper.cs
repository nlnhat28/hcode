using Dapper;
using System.Reflection;

namespace HCode.Domain
{
    /// <summary>
    /// Các phương thức trợ giúp tầng infrastructure
    /// </summary>
    /// Created by: nlnhat (20/07/2023)
    public class InfrastructureHelper
    {
        /// <summary>
        /// Check xem 1 property có trong 1 class hay không
        /// </summary>
        /// <typeparam name="T">Class check</typeparam>
        /// <param name="propertyName">Tên property</param>
        /// <returns>True nếu có, false nếu không</returns>
        /// Created by: nlnhat (20/07/2023)
        public static bool IsPropertyInClass<T>(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return false;

            var type = typeof(T);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (propertyName.Contains(property.Name))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Tạo param từ các properties trong class (entity)
        /// </summary>
        /// <typeparam name="TEntity">Thực thể</typeparam>
        /// <param name="entity">Đối tượng</param>
        /// <returns>Param được add các tham số từ properties của class</returns>
        /// Created by: nlnhat (20/07/2023)
        public static DynamicParameters GetParamFromEntity<TEntity>(TEntity? entity)
        {
            var param = new DynamicParameters();

            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties();

            foreach (var property in properties)
            {
                var notMapped = property.GetCustomAttribute<NotMappedAttribute>();
                if (notMapped == null)
                {
                    var propertyName = "p_" + property.Name;
                    var propertyValue = entity != null ? property.GetValue(entity) : null;
                    param.Add(propertyName, propertyValue);
                }    
            }

            return param;
        }
        /// <summary>
        /// Tạo query logic 
        /// </summary>
        /// <param name="logicType">Enum logic type</param>
        /// <returns>Chuỗi AND hoặc OR</returns>
        public static string GetLogicTypeQuery(LogicType logicType)
        {
            return logicType switch
            {
                LogicType.And => "AND",
                LogicType.Or => "OR",
                _ => "AND",
            };
        }
        /// <summary>
        /// Tạo query sort type 
        /// </summary>
        /// <param name="sortType">Enum sort type</param>
        /// <returns>Chuỗi ASC hoặc DESC</returns>
        public static string GetSortTypeQuery(SortType sortType)
        {
            return sortType switch
            {
                SortType.Ascending => "ASC",
                SortType.Descending => "DESC",
                _ => "ASC",
            };
        }
        /// <summary>
        /// Thoát ký tự đặc biệt trong Mysql
        /// </summary>
        /// <param name="value">Giá trị lọc</param>
        /// <returns>Giá trị được thay thế ký tự đặc biệt bằng ký tự hợp lệ</returns>
        /// Created by: nlnhat (29/08/2023)
        public static string EscapeSpecialCharacters(string value)
        {
            var replacements = new Dictionary<char, string>
            {
                {'\\', "\\\\"} , //    \  ->  \\
                {'`', "``"},     //    `  ->  ``
                {'\'', "\\\'"},  //    '  ->  \'
                {'"', "\\\""},   //    "  ->  \"
                {'%', "\\%"},    //    %  ->  \%
                {'_', "\\_"}     //    _  ->  \_
            };

            foreach (var replacement in replacements)
            {
                value = value.Replace(replacement.Key.ToString(), replacement.Value);
            }

            return value;
        }
    }
}