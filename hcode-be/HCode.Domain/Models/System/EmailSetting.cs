namespace HCode.Domain
{
    /// <summary>
    /// Setting cho Email
    /// </summary>
    public class EmailSetting
    {
        /// <summary>
        /// Địa chỉ người gửi
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }
    }
}
