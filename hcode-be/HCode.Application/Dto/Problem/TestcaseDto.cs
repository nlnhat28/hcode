
using System.Text.Json;

namespace HCode.Application
{
    /// <summary>
    /// Dto test case
    /// </summary>
    public class TestcaseDto
    {
        #region Properties
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
        public List<object>? Inputs { get; set; }
        /// <summary>
        /// Kết quả mong đợi
        /// </summary>
        public object? ExpectedOutput { get; set; }
        /// <summary>
        /// Cho hiện hay không
        /// </summary>
        public bool AllowView { get; set; } = true;
        /// <summary>
        /// Kích hoạt hay không
        /// </summary>
        public bool IsActivated { get; set; } = true;
        /// <summary>
        /// Submission của testcase
        /// </summary>
        [AutoMapper.Configuration.Annotations.Ignore]
        public SubmissionResponse? Status { get; set; }
        #endregion

        #region Methods
        public string SerializeInputs()
        {
            //var json = JsonSerializer.Serialize(Inputs);
            var json = Inputs != null ? string.Join("|", Inputs) : string.Empty;
            return json;
        }
        #endregion
    }
}
