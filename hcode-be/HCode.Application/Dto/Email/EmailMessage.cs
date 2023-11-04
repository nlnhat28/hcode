namespace HCode.Application
{
    /// <summary>
    /// Dùng cho gửi mail
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Người nhận
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Chủ đề
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }
    }
}
