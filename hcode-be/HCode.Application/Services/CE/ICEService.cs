
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Interface thực thi code
    /// </summary>
    public interface ICEService : ICoreService
    {
        /// <summary>
        /// Gửi request đến server
        /// </summary>
        /// <param name="uri">Full Uri</param>
        /// <param name="method">Http method</param>
        /// <param name="content">Nội dung</param>
        /// <returns></returns>
        Task<BaseResponse> SendAsync(string uri, HttpMethod method, string content);
        /// <summary>
        /// Chạy 1 submission
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResponse> CreateSubmissionAsync(SubmissionParam param);
        /// <summary>
        /// Tạo nhiều submission
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResponse> CreateBatchAsync(List<SubmissionParam> param);
        /// <summary>
        /// Lấy nhiều submission
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        Task<BaseResponse> GetBatchAsync(List<string> tokens);
    }
}
