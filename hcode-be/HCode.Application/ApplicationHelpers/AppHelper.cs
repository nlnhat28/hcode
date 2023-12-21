using AutoMapper.Configuration.Annotations;
using HCode.Domain;
using Microsoft.Extensions.Localization;
using OfficeOpenXml.Drawing.Chart;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HCode.Application
{
    /// <summary>
    /// Các phương thức trợ giúp tầng Appication
    /// </summary>
    /// Created by: nlnhat (22/07/2023)
    public class AppHelper
    {
        #region Methods
        /// <summary>
        /// Lấy tên đơn vị thời gian
        /// </summary>
        /// <param name="timeUnit">Enum đơn vị thời gian</param>
        /// <returns>Tên đơn vị thời gian</returns>
        /// Created by: nlnhat (22/08/2023)
        public static string GetTimeUnitName(TimeUnit? timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Day => "Ngày",
                TimeUnit.Month => "Tháng",
                TimeUnit.Year => "Năm",
                _ => string.Empty,
            };
        }
        /// <summary>
        /// Lấy tên phép tính
        /// </summary>
        /// <param name="_operator">Enum phép tính</param>
        /// <returns>Tên phép tính</returns>
        /// Created by: nlnhat (22/08/2023)
        public static string GetOperatorName(Operator? _operator)
        {
            return _operator switch
            {
                Operator.Multiple => "*",
                Operator.Divide => "/",
                _ => string.Empty,
            };
        }
        /// <summary>
        /// Lấy danh sách các vai trò khác
        /// </summary>
        /// <param name="isCustomer">Là khách hàng hay không</param>
        /// <param name="isProvider">Là nhà cung cấp hay không</param>
        /// <returns>Danh sách các vai trò khác</returns>
        /// Created by: nlnhat (22/07/2023)
        public static List<string> GetOtherRoles(bool? isCustomer, bool? isProvider)
        {
            var otherRoles = new List<string>();
            if (isCustomer == true)
            {
                otherRoles.Add("Khách hàng");
            }
            if (isProvider == true)
            {
                otherRoles.Add("Nhà cung cấp");
            };
            return otherRoles;
        }
        /// <summary>
        /// Lấy tên các vai trò khác
        /// </summary>
        /// <param name="isCustomer">Là khách hàng hay không</param>
        /// <param name="isProvider">Là nhà cung cấp hay không</param>
        /// <returns>Tên các vai trò khác</returns>
        /// Created by: nlnhat (22/07/2023)
        public static string GetOtherRolesName(bool? isCustomer, bool? isProvider)
        {
            var otherRoles = GetOtherRoles(isCustomer, isProvider);
            return string.Join(", ", otherRoles);
        }
        /// <summary>
        /// Tạo mô tả đơn vị chuyển đổi
        /// </summary>
        /// <param name="unitName">Tên đơn vị tính</param>
        /// <param name="destinationUnitName">Tên đơn vị chuyển đổi</param>
        /// <param name="conversionOperator">Phép tính</param>
        /// <param name="rate">Tỉ lệ chuyển đổi</param>
        /// <returns>Mô tả đơn vị chuyển đổi</returns>
        public static string GetConversionUnitDescription(string? unitName, string? destinationUnitName, Operator? conversionOperator, decimal? rate)
        {
            if (unitName != null && destinationUnitName != null && conversionOperator != null && rate != null)
            {
                return conversionOperator switch
                {
                    Operator.Multiple => $"1 {unitName} = {rate} * {destinationUnitName}",
                    Operator.Divide => $"1 {unitName} = 1 / {rate} {destinationUnitName}",
                    _ => string.Empty
                };
            }
            return string.Empty;
        }
        /// <summary>
        /// Chuyển thời gian từ utc sang local
        /// </summary>
        /// <param name="dateTime">Thời gian utc</param>
        /// <returns>Thời gian local</returns>
        /// Created by: nlnhat (22/07/2023)
        public static DateTime? ConvertDateUtcToLocal(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var localTimeZone = TimeZoneInfo.Local;
                var localDateTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)dateTime, localTimeZone);
                localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);

                return localDateTime;
            }
            return null;
        }
        /// <summary>
        /// Chuyển thời gian từ local sang utc
        /// </summary>
        /// <param name="dateTime">Thời gian local</param>
        /// <returns>Thời gian utc</returns>
        /// Created by: nlnhat (22/07/2023)
        public static DateTime? ConvertDateLocalToUtc(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var localDateTimeOffset = new DateTimeOffset((DateTime)dateTime);

                var utcDateTimeOffset = localDateTimeOffset.ToUniversalTime();
                var utcDateTime = utcDateTimeOffset.UtcDateTime;

                return utcDateTime;
            }
            return null;
        }
        /// <summary>
        /// Đo khoảng cách từ thời gian tạo đến thời điểm hiện tại để xem mới hay không
        /// </summary>
        /// <param name="createdTime">Thời gian tạo</param>
        /// <param name="days">Nhỏ hơn số ngày này thì IsNew</param>
        /// <returns></returns>
        public static bool IsNew(DateTime? createdTime, int? days = 7)
        {
            if (createdTime != null)
            {
                var difference = (TimeSpan)(DateTime.UtcNow - createdTime);
                return difference.TotalDays <= days;
            }
            return false;
        }
        /// <summary>
        /// Lấy chữ cái đầu mỗi từ trong chuỗi. Vd: Nguyên vật liệu -> Nvl
        /// </summary>
        /// <param name="text">Chuỗi đầu vào</param>
        /// <returns>Chuỗi mới tạo thành từ chữ cái đầu mỗi chuỗi</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string GetFirstCharEachWord(string text)
        {
            var trimmedAndNormalized = Regex.Replace(text.Trim(), @"\s+", " ");

            var result = new string(trimmedAndNormalized.Split(' ').Select(word => word[0]).ToArray());
            return result;
        }
        /// <summary>
        /// Chuyển Tiếng Việt có dấu sang không dấu
        /// </summary>
        /// <param name="input">Chuỗi đầu vào</param>
        /// <returns>Chuỗi mới không dấu. Ví dụ: Tiếng Việt -> Tieng Viet</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string ConvertDiacritics(string input)
        {
            var normalized = input.Normalize(NormalizationForm.FormD);
            normalized = normalized.Replace("Đ", "D").Replace("đ", "d");

            var chars = normalized.Where(
                character => CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark);

            var result = new string(chars.ToArray());
            return result;
        }
        /// <summary>
        /// Loại bỏ kí tự không phải là chữ cái
        /// </summary>
        /// <param name="input">Chuỗi đầu vào</param>
        /// <returns>Chuỗi mới chỉ bao gồm chữ cái. Ví dụ: new1@ string -> newstring</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string RemoveNonLetters(string input)
        {
            var pattern = "[^a-zA-Z]";
            var result = Regex.Replace(input, pattern, "");
            return result;
        }
        /// <summary>
        /// Tạo prefix code từ chữ cái đầu của tên
        /// </summary>
        /// <param name="name">Tên</param>
        /// <returns>Chuỗi prefix code được lấy từ các chữ cái đầu của tên, loại bỏ dấu, ký tự đặc biệt và in hoa</returns>
        /// Created by: nlnhat (17/08/2023)
        public static string GetPrefixCode(string name)
        {
            var prefix = GetFirstCharEachWord(name);
            prefix = ConvertDiacritics(prefix);
            prefix = RemoveNonLetters(prefix);
            prefix = prefix.ToUpper();
            return prefix;
        }
        /// <summary>
        /// Hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>HashedPassword: Mật khẩu được mã hoá; Salt: Salt</returns>
        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return (hashedPassword, salt);
        }
        /// <summary>
        /// Convert độ khó sang tên
        /// </summary>
        /// <param name="difficulty">Enum Difficulty</param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static string? ConvertToDifficultyName(Difficulty? difficulty, IStringLocalizer<Resource> resource)
        {
            var difficultyName = difficulty switch
            {
                Difficulty.Easy => resource["DifficultyEasy"],
                Difficulty.Medium => resource["DifficultyMedium"],
                Difficulty.Hard => resource["DifficultyHard"],
                _ => string.Empty,
            };

            return difficultyName;
        }
        /// <summary>
        /// Convert json string to list object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<object>? ConvertToObjects(string? json)
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                var objs = JsonSerializer.Deserialize<List<object>>(json);
                return objs;
            }
            return new List<object>();
        }
        /// <summary>
        /// Convert string to object
        /// </summary>
        /// <param name="any"></param>
        /// <returns></returns>
        public static object? ConvertToObject(string? any)
        {
            if (!string.IsNullOrWhiteSpace(any))
            {
                var obj = JsonSerializer.Deserialize<object>(any);
                return obj;
            }
            return any;
        }


        /// <summary>
        /// Lấy tất cả các properties nối thành chuỗi ngăn cách bởi dấu ,
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allowIgnore">Lấy cả các property có [IgnoreAttribute]</param>
        /// <param name="allowNotMapped">Lấy cả các property có [NotMappedAttribute]</param>
        /// <returns>Chuỗi các properties ngăn cách bởi dấu ,</returns>
        public static string? GetFieldsToString<T>(bool? allowIgnore = false, bool? allowNotMapped = false)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var fields = new List<string>();

            foreach (var property in properties)
            {
                if (allowIgnore != null)
                {
                    var ignore = property.GetCustomAttribute<IgnoreAttribute>();
                    // Không lấy những [IgnoreAttribute] thì bỏ qua property có
                    if (allowIgnore == false && ignore != null)
                    {
                        continue;
                    }
                }
                if (allowNotMapped != null)
                {
                    var notMapped = property.GetCustomAttribute<NotMappedAttribute>();
                    // Không lấy những [NotMappedAttribute] thì bỏ qua property có
                    if (allowNotMapped == true && notMapped != null)
                    {
                        continue;
                    }
                }

                fields.Add(property.Name);
            }

            var result = string.Join(",", fields);
            return result;
        }
        #endregion
    }
}
