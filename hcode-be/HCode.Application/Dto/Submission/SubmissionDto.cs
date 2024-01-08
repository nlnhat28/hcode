

using AutoMapper.Configuration.Annotations;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Nộp code
    /// </summary>
    public class SubmissionDto
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid SubmissionId { get; set; }
        public Guid Id
        {
            get { return SubmissionId; }
            set { SubmissionId = value; }
        }
        public Guid? ParentId { get; set; }
        public string? SourceCode { get; set;}
        public Guid LanguageId { get; set;}
        #endregion
    }
}