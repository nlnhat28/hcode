namespace HCode.Domain
{
    /// <summary>
    /// Setting cho Jwt
    /// </summary>
    public class JwtConfig
    {   
        /// <summary>
        /// Issuer
        /// </summary>
        public string? Issuer { get; set; }
        /// <summary>
        /// Audience
        /// </summary>
        public string? Audience { get; set; }
        /// <summary>
        /// Secret key
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Thời gian token hết hạn
        /// </summary>
        public int ExpireToken { get; set; }
    }
}
