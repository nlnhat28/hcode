namespace HCode.Domain
{
    /// <summary>
    /// Setting cho auth
    /// </summary>
    public class AuthConfig
    {
        /// <summary>
        /// Thời gian hết hạn verify code
        /// </summary>
        public int VerifyTimeOut { get; set; }
        /// <summary>
        /// Độ lớn verify code
        /// </summary>
        public int MaxVerifyCode { get; set; }
        /// <summary>
        /// Độ dài verify code
        /// </summary>
        public int PaddingVerifyCode { get; set; }
    }
}
