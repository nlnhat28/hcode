using Dapper;
using System.Data.Common;
using System.Drawing.Printing;
using System.Reflection;
using static Dapper.SqlMapper;

namespace HCode.Domain
{
    /// <summary>
    /// Các phương thức trợ giúp tầng infrastructure
    /// </summary>
    /// Created by: nlnhat (20/07/2023)
    public class InfraHelper
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
        /// Tạo câu truy vấn tìm kiếm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keySearch"></param>
        /// <returns></returns>
        public static string GenerateQuerySearch<T>(string? keySearch)
        {
            var query = string.Empty;
            if (!string.IsNullOrEmpty(keySearch))
            {
                keySearch = EscapeSpecialCharacters(keySearch);
                var items = new List<string>();

                var entityType = typeof(T);
                var canSearch = entityType.GetCustomAttribute<CanSearchAttribute>();

                if (canSearch != null)
                {
                    var columns = canSearch.Columns;

                    if (columns != null && columns?.Count > 0)
                    {
                        foreach (var column in columns)
                        {
                            var queryItem = $"({column} LIKE '%{keySearch}%')";
                            items.Add(queryItem);
                        }
                    }
                }    

                if (items.Count > 0)
                {
                    query = $" AND ({string.Join(" OR ", items)}) ";
                }
            }
            
            return query;
        }
        /// <summary>
        /// Tạo các câu truy vấn Orderby 
        /// </summary>
        /// <param name="sortModels">Danh sách các models sắp xếp</param>
        /// <returns>Các câu truy vấn Orderby</returns>
        /// Created by: nlnhat (19/08/2023)
        public static string GenerateQuerySort<T>(List<SortModel>? sortModels)
        {
            var queryItems = new List<string>();
            var query = string.Empty;

            if (sortModels != null)
            {
                foreach (var sortModel in sortModels)
                {
                    if (sortModel == null)
                        continue;

                    var column = sortModel.ColumnName;
                    var sortType = sortModel.SortType;

                    // Phân loại sắp xếp
                    var sortTypeString = GetSortTypeQuery(sortType);

                    string baseQuery = $"{column} {sortTypeString}";
                    queryItems.Add(baseQuery);
                }
                query = queryItems.Count > 0 ? $"ORDER BY {string.Join(", ", queryItems)} " : string.Empty;
            }

            return query;
        }
        /// <summary>
        /// Tạo các câu truy vấn lọc
        /// </summary>
        /// <param name="filterModels">Danh sách các models lọc</param>
        /// <returns>Các câu truy vấn lọc</returns>
        /// Created by: nlnhat (19/08/2023)
        public static string GenerateQueryFilter<T>(List<FilterModel>? filterModels)
        {
            var query = string.Empty;
            var orQueries = new List<string>();
            var orQuery = string.Empty;

            if (filterModels != null)
            {
                foreach (var filterModel in filterModels)
                {
                    var column = filterModel.ColumnName;
                    var compare = filterModel.CompareType;
                    var logic = filterModel.LogicType;
                    var value = filterModel.FilterValue;
                    if (!string.IsNullOrEmpty(value?.ToString()))
                    {
                        value = EscapeSpecialCharacters(value?.ToString());
                    }

                    var queryItem = string.Empty;

                    // Phân loại so sánh
                    switch (compare)
                    {
                        case CompareType.Empty:
                            queryItem = $"({column} IS NULL OR {column} = '')";
                            break;
                        case CompareType.Equal:
                            queryItem = $"({column} LIKE '{value}')";
                            break;
                        case CompareType.Contain:
                            queryItem = $"({column} LIKE '%{value}%')";
                            break;
                        case CompareType.NotContain:
                            queryItem = $"({column} NOT LIKE '%{value}%')";
                            break;
                        case CompareType.Less:
                            queryItem = $"({column} < {value})";
                            break;
                        case CompareType.More:
                            queryItem = $"({column} > {value})";
                            break;
                        case CompareType.LessEqual:
                            queryItem = $"({column} < {value} OR {column} = {value})";
                            break;
                        case CompareType.MoreEqual:
                            queryItem = $"({column} > {value} OR {column} = {value})";
                            break;
                        case CompareType.NotEqual:
                            queryItem = $"({column} <> {value})";
                            break;
                        case CompareType.StartWith:
                            queryItem = $"({column} LIKE '{value}%')";
                            break;
                        case CompareType.EndWidth:
                            queryItem = $"({column} LIKE '%{value}')";
                            break;
                        default:
                            break;
                    }

                    // Phân loại logic
                    switch (logic)
                    {
                        case LogicType.And:
                            queryItem = $" AND {queryItem} ";
                            query += queryItem;
                            break;
                        case LogicType.Or:
                            orQueries.Add(queryItem);
                            break;
                        default:
                            queryItem = $" AND {queryItem} ";
                            query += queryItem; 
                            break;
                    }
                }

                if (orQueries.Count > 1)
                {
                    orQuery = $" AND ({string.Join(" OR ", orQueries)}) ";
                }

                query += orQuery;
            }

            return query;
        }
        /// <summary>
        /// Thoát ký tự đặc biệt trong Mysql
        /// </summary>
        /// <param name="value">Giá trị lọc</param>
        /// <returns>Giá trị được thay thế ký tự đặc biệt bằng ký tự hợp lệ</returns>
        /// Created by: nlnhat (29/08/2023)
        public static string? EscapeSpecialCharacters(string? value)
        {
            if (!string.IsNullOrEmpty(value))
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
            }

            return value;
        }
        /// <summary>
        /// Phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagingModel"></param>
        /// <returns></returns>
        public static IEnumerable<T> Paging<T>(IEnumerable<T>? list, ref PagingModel? pagingModel)
        {
            if (list == null || pagingModel?.PageNumber <= 0 || pagingModel?.PageSize <= 0)
            {
                return new List<T>();
            }

            // null hoặc size < 0 thì trả về tất
            if (pagingModel == null)
            {
                return list;
            }
            else
            {
                var totalPage = (int)Math.Ceiling((double)list.Count() / pagingModel.PageSize); ; // >= 1

                var pageNumber = pagingModel.PageNumber = Math.Min(pagingModel.PageNumber, totalPage); // >= 1

                var result = list.Skip((pageNumber - 1) * pagingModel.PageSize).Take(pagingModel.PageSize);

                return result;
            }
        }
    }
}
