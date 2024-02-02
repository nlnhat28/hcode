
using AutoMapper;
using HCode.Domain;
using MailKit.Security;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Cmp;

namespace HCode.Application
{
    // Dịch vụ gửi mail
    public class EmailService : CoreService, IEmailService
    {
        #region Fields
        /// <summary>
        /// Email setting
        /// </summary>
        private readonly EmailConfig _emailConfig;
        #endregion

        #region Constructors
        public EmailService(IOptions<EmailConfig> emailConfig, IStringLocalizer<Resource> resource, IMapper mapper)
            : base(resource, mapper)
        {
            _emailConfig = emailConfig.Value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gửi mail
        /// </summary>
        public async Task<BaseResponse> SendEmailAsync(EmailMessage emailMessage)
        {
            var builder = new BodyBuilder
            {
                HtmlBody = emailMessage.Content
            };

            var message = new MimeMessage
            {
                Sender = new MailboxAddress(_emailConfig.DisplayName, _emailConfig.Email),
                Subject = emailMessage.Subject,
                Body = builder.ToMessageBody()
            };
            message.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.Email));
            message.To.Add(MailboxAddress.Parse(emailMessage.To));

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailConfig.Email, _emailConfig.Password);
                var result = await smtp.SendAsync(message);
                return new BaseResponse(devMsg: result);
            }
            catch (Exception exception)
            {
                await smtp.DisconnectAsync(true);
                return new BaseResponse(false, exception.Message, exception.Data);
            }
            #endregion
        }
    }
}