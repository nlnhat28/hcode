using Dapper;
using HCode.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
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
        public static DynamicParameters GetParamFromEntity<TEntity>(TEntity? entity, int? index = null)
        {
            var param = new DynamicParameters();

            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties();

            var id = index != null ? $"_{index}" : string.Empty;

            foreach (var property in properties)
            {
                var notMapped = property.GetCustomAttribute<NotMappedAttribute>();
                if (notMapped == null)
                {
                    var propertyName = $"p_{property.Name}{id}";
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
                        case CompareType.Null:
                            queryItem = $"({column} IS NULL)";
                            break;
                        case CompareType.NotNull:
                            queryItem = $"({column} IS NOT NULL)";
                            break;
                        case CompareType.Empty:
                            queryItem = $"({column} = '')";
                            break;
                        case CompareType.NullOrEmpty:
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
                        case CompareType.AccountId:
                            queryItem = $"({column} = '{value}')";
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
        /// <summary>
        /// Tạo script insert
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static (string script, DynamicParameters param) ScriptInsert<TEntity>(
            TEntity entity, int? index = null, bool? getId = true)
        {
            var script = "INSERT INTO {0}(\n{1}) VALUES (\n{2});";
            var param = new DynamicParameters();
            var entityType = typeof(TEntity);
            var columns = new List<string>();
            var parameters = new List<string>();

            var tableAttribute = entityType.GetCustomAttribute<TableAttribute>();
            var table = tableAttribute != null ? tableAttribute.Name : typeof(TEntity).Name;
            var tableId = $"{table}Id";

            var scriptAttr = entityType.GetCustomAttribute<ScriptAttribute>();
            var auditFields = new List<string>() { "CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime" };
            var isIgnoreAudit = scriptAttr != null && scriptAttr.IsIgnoreAudit == true;

            var id = index != null ? $"_{index}" : string.Empty;

            var properties = entityType.GetProperties();

            foreach (var property in properties)
            {
                var notMapped = property.GetCustomAttribute<NotMappedAttribute>();
                var ignoreAuditField = isIgnoreAudit && auditFields.Contains(property.Name);

                if (notMapped == null && !ignoreAuditField)
                {
                    columns.Add($"`{property.Name}`");

                    var propertyName = $"p_{property.Name}{id}";
                    var paramName = $"@{propertyName}";

                    parameters.Add(paramName);

                    var propertyValue = entity != null ? property.GetValue(entity) : null;
                    param.Add(propertyName, propertyValue);
                };

                var keyAttribute = property.GetCustomAttribute<KeyAttribute>();
                if (keyAttribute != null)
                {
                    tableId = property.Name;
                }
            }

            script = string.Format(script, table, string.Join(",\n", columns), string.Join(",\n", parameters));

            if (getId == true)
            {
                script += $"\nSELECT @p_{tableId} AS {tableId}";
            }

            return (script, param);
        }
        /// <summary>
        /// Tạo script update
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static (string script, DynamicParameters param) ScriptUpdate<TEntity>(
            TEntity entity, int? index = null, string? whereClause = null)
        {
            var script = "UPDATE {0} SET\n{1}\n{2};";
            var param = new DynamicParameters();
            var entityType = typeof(TEntity);
            var expressions = new List<string>();  // Id = @p_Id
            var whereClauses = new List<string>();

            var tableAttribute = entityType.GetCustomAttribute<TableAttribute>();
            var table = tableAttribute != null ? tableAttribute.Name : typeof(TEntity).Name;
            var tableId = $"{table}Id";

            var scriptAttr = entityType.GetCustomAttribute<ScriptAttribute>();
            var auditFields = new List<string>() { "CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime" };
            var isIgnoreAudit = scriptAttr != null && scriptAttr.IsIgnoreAudit == true;

            var id = index != null ? $"_{index}" : string.Empty;

            var paramId = $"@p_{tableId}{id}";

            var properties = entityType.GetProperties();

            foreach (var property in properties)
            {
                var notMapped = property.GetCustomAttribute<NotMappedAttribute>();
                var ignoreAuditField = isIgnoreAudit && auditFields.Contains(property.Name);

                if (notMapped == null && !ignoreAuditField)
                {
                    var propertyName = $"p_{property.Name}{id}";  //p_Id_1
                    var paramName = $"@{propertyName}";   // @p_Id_1
                    var propertyValue = entity != null ? property.GetValue(entity) : null;

                    var scriptAttribute = property.GetCustomAttribute<ScriptAttribute>();
                    if (scriptAttribute != null)
                    {
                        if (scriptAttribute.IsWhereUpdate)
                        {
                            var whereExpression = $"`{property.Name}` = {paramName}";
                            whereClauses.Add(whereExpression);
                            param.Add(propertyName, propertyValue);

                            if (scriptAttribute.IsNotUpdate)
                            {
                                continue;
                            }
                        }
                        else if (scriptAttribute.IsNotUpdate)
                        {
                            continue;
                        }
                    }

                    param.Add(propertyName, propertyValue);

                    var keyAttribute = property.GetCustomAttribute<KeyAttribute>();

                    if (keyAttribute != null)
                    {
                        tableId = property.Name;
                        paramId = paramName;
                        continue;
                    }

                    var expression = $"`{property.Name}` = {paramName}";
                    expressions.Add(expression);
                };
            }

            var whereScript = whereClause;

            if (whereScript == null && whereClauses.Count > 0)
            {
                whereScript = $"WHERE {string.Join(" AND ", whereClauses)}";
            }

            whereScript ??= $"WHERE {tableId} = {paramId}";

            script = string.Format(script, table, string.Join(",\n", expressions), whereScript);

            return (script, param);
        }
        /// <summary>
        /// Tạo script update theo cột
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static (string script, DynamicParameters param) ScriptUpdateByColumn<TEntity>(
            TEntity entity, List<string> columns, int? index = null, string? whereClause = null)
        {
            var script = "UPDATE {0} SET\n{1}\n{2};";
            var param = new DynamicParameters();
            var entityType = typeof(TEntity);
            var expressions = new List<string>();  // Id = @p_Id
            var whereClauses = new List<string>();

            var tableAttribute = entityType.GetCustomAttribute<TableAttribute>();
            var table = tableAttribute != null ? tableAttribute.Name : typeof(TEntity).Name;
            var tableId = $"{table}Id";

            var scriptAttr = entityType.GetCustomAttribute<ScriptAttribute>();
            var auditFields = new List<string>() { "CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime" };
            var isIgnoreAudit = scriptAttr != null && scriptAttr.IsIgnoreAudit == true;

            var id = index != null ? $"_{index}" : string.Empty;

            var paramId = $"@p_{tableId}{id}";

            var properties = entityType.GetProperties();

            foreach (var property in properties)
            {
                if (!columns.Contains(property.Name))
                {
                    continue;
                }

                var notMapped = property.GetCustomAttribute<NotMappedAttribute>();
                var ignoreAuditField = isIgnoreAudit && auditFields.Contains(property.Name);

                if (notMapped == null && !ignoreAuditField)
                { 
                    var propertyName = $"p_{property.Name}{id}";  //p_Id_1
                    var paramName = $"@{propertyName}";   // @p_Id_1
                    var propertyValue = entity != null ? property.GetValue(entity) : null;

                    var scriptAttribute = property.GetCustomAttribute<ScriptAttribute>();
                    if (scriptAttribute != null)
                    {
                        if (scriptAttribute.IsWhereUpdate)
                        {
                            var whereExpression = $"`{property.Name}` = {paramName}";
                            whereClauses.Add(whereExpression);
                            param.Add(propertyName, propertyValue);

                            if (scriptAttribute.IsNotUpdate)
                            {
                                continue;
                            }
                        }
                        else if (scriptAttribute.IsNotUpdate)
                        {
                            continue;
                        }
                    }

                    param.Add(propertyName, propertyValue);

                    var keyAttribute = property.GetCustomAttribute<KeyAttribute>();

                    if (keyAttribute != null)
                    {
                        tableId = property.Name;
                        paramId = paramName;
                        continue;
                    }

                    var expression = $"`{property.Name}` = {paramName}";
                    expressions.Add(expression);
                };
            }

            var whereScript = whereClause;

            if (whereScript == null && whereClauses.Count > 0)
            {
                whereScript = $"WHERE {string.Join(" AND ", whereClauses)}";
            }

            whereScript ??= $"WHERE {tableId} = {paramId}";


            if (expressions.Count == 0)
            {
                var expression = $"{tableId} = {tableId}";
                expressions.Add(expression);

                whereScript += "AND FALSE";
            }

            script = string.Format(script, table, string.Join(",\n", expressions), whereScript);

            return (script, param);
        }
    }
}
