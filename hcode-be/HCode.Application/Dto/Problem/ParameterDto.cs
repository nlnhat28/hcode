
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Dto tham số
    /// </summary>
    public class ParameterDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid ParameterId { get; set; }
        /// <summary>
        /// Id problem
        /// </summary>
        public Guid ProblemId { get; set; }
        /// <summary>
        /// Tên tham số
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// Thứ tự
        /// </summary>
        public int ParameterOrder { get; set; }
        /// <summary>
        /// Kiểu dữ liệu
        /// </summary>
        public DataType DataType { get; set; }
    }
}
