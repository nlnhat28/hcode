
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Interface email
    /// </summary>
    public interface IEmailService : ICoreService
    {
        /// <summary>
        /// Gửi mail
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        Task<BaseResponse> SendEmailAsync(EmailMessage emailMessage);
    }
}
