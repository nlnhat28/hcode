
namespace HCode.Domain
{
    /// <summary>
    /// Lớp ngôn ngữ lập trình
    /// </summary>
    public class Language : IHasEntityId
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid LanguageId { get; set; }
        public Guid Id
        {
            get { return LanguageId; }
            set { LanguageId = value; }
        }
        /// <summary>
        /// Tên ngôn ngữ
        /// </summary>
        public string? LanguageName { get; set; }
        /// <summary>
        /// Id gửi đến judge0
        /// </summary>
        public int JudgeId { get; set; }
        #endregion

        #region Constructor
        #endregion
    }
}