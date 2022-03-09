using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using HexMaster.Email.Abstractions.Services;
using HexMaster.Email.Configuration;
using HexMaster.Email.DomainModels;
using HexMaster.Email.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HexMaster.Email.Services
{
    public class MailService:IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly Lazy<SmtpClient> _client;

        public async Task SendAsync(Message mail)
        {
            if (!mail.IsValid())
            {
                throw new MailMessageInvalidException();
            }
            _logger.LogInformation("Starting to send mail batch of mail with subject {subject}", mail.Subject);
            try
            {
                while (mail.Recipients.Count(r => !r.IsCompleted && !r.IsFailed) > 0)
                {
                    var recipient = mail.Recipients.OrderBy(r=> r.Attempts).First(r => !r.IsCompleted && !r.IsFailed);
                    recipient.Attempt();

                    _logger.LogInformation("Composing mail for recipient {recipient}", recipient.EmailAddress);

                    var fromAddress = new MailAddress(mail.Sender.EmailAddress, mail.Sender.Name);
                    var toAddress = new MailAddress(recipient.EmailAddress, recipient.Name);
                    var mailMessage = new MailMessage(fromAddress, toAddress);
                    if (!string.IsNullOrWhiteSpace(mail.Sender.ReplyToAddress))
                    {
                        mailMessage.ReplyToList.Add(mail.Sender.ReplyToAddress);
                    }

                    mailMessage.Subject = SubstituteSubjectTokens(mail.Subject, recipient);

                    var defaultBody = mail.Bodies.First(b=> b.IsDefault);
                    mailMessage.Body = SubstituteSubjectTokens(defaultBody.Content, recipient);
                    mailMessage.IsBodyHtml = defaultBody.IsHtml;

                    foreach (var body in mail.Bodies.Where(b => !b.IsDefault))
                    {
                        var mediaType = body.IsHtml ? "text/html" : "text/plain";
                        var alternativeView = AlternateView.CreateAlternateViewFromString(body.Content, new ContentType(mediaType));
                        mailMessage.AlternateViews.Add(alternativeView);
                    }

                    try
                    {
                        _logger.LogInformation("Composing mail for recipient {recipient} complete, now attempting to send", recipient.EmailAddress);
                        await _client.Value.SendMailAsync(mailMessage);
                        recipient.Completed();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Sending mail to recipient {recipient} failed", recipient.EmailAddress);
                    }
                }
            }
            finally
            {
                _logger.LogInformation("Sending mail batch complete");
            }
        }

        private static SmtpClient CreateClient(IOptions<EmailOptions> options)
        {
            var clientOptions = options.Value;
            var smtpClient =  new SmtpClient(clientOptions.SmtpHost);
            if (clientOptions.SmtpPort.HasValue)
            {
                smtpClient.Port = clientOptions.SmtpPort.Value;
            }

            if (clientOptions.UseSsl.HasValue)
            {
                smtpClient.EnableSsl = clientOptions.UseSsl.Value;
            }

            if (!string.IsNullOrWhiteSpace(clientOptions.Username) ||
                !string.IsNullOrWhiteSpace(clientOptions.Password))
            {
                smtpClient.Credentials= new NetworkCredential(clientOptions.Username, clientOptions.Password);
            }
            return smtpClient;
        }
        private static string SubstituteSubjectTokens(string template, Recipient recipient)
        {
            var subtitutedContent = template;
            foreach (var substitute in recipient.Substitutions)
            {
                subtitutedContent = subtitutedContent.Replace(substitute.Key, substitute.Value);
            }

            return subtitutedContent;
        }

        public MailService(IOptions<EmailOptions> options, ILogger<MailService> logger)
        {
            _logger = logger;
            _client = new Lazy<SmtpClient>(() => CreateClient(options));
        }
    }
}