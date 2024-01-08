
namespace HCode.Domain
{
    /// <summary>
    /// Test case cho problem
    /// </summary>
    public class Testcase
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid TestcaseId { get; set; }
        /// <summary>
        /// Id problem
        /// </summary>
        public Guid ProblemId { get; set; }
        /// <summary>
        /// Thứ tự
        /// </summary>
        public int TestcaseOrder { get; set; }
        /// <summary>
        /// Danh sách giá trị tham số
        /// </summary>
        public string? Inputs { get; set; }
        /// <summary>
        /// Kết quả mong đợi
        /// </summary>
        public string? ExpectedOutput { get; set; }
        /// <summary>
        /// Cho hiện hay không
        /// </summary>
        public bool AllowView { get; set; } = true;
        /// <summary>
        /// Kích hoạt hay không
        /// </summary>
        public bool IsActivated { get; set; } = true;
    }
}
