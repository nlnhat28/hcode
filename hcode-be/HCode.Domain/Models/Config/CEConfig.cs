

namespace HCode.Domain
{
    /// <summary>
    /// Config server thực thi code
    /// </summary>
    public class CEConfig
    {
        /// <summary>
        /// Base url: "https://judge0-ce.p.rapidapi.com",
        /// 
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// Api host
        /// </summary>
        public string? ApiHost { get; set; }
        /// <summary>
        /// Api key
        /// </summary>
        public string? ApiKey { get; set; }
        /// <summary>
        /// Encode source code or not
        /// </summary>
        public bool? IsBase64Encode { get; set; } = true;
    }
}
