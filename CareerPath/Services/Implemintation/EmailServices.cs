using CareerPath.Services.Abstraction;
using CareerPath.Sittings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CareerPath.Services.Implemintation
{
    public class EmailServices : IEmailServices
    {

        private readonly EmailSittings _emailSittings;
        public EmailServices(IOptions<EmailSittings> mail)
        {
            this._emailSittings = mail.Value;
        }

        public async Task sendEmailAsync(string mailTo, string Subject, string body, IList<IFormFile> attatcments = null)
        {

            var email = new MimeMessage
            {

                Sender = MailboxAddress.Parse(_emailSittings.Email),
                Subject = Subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var bilder = new BodyBuilder();
            if (attatcments != null)
            {
                byte[] filebytes;
                foreach (var file in attatcments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        filebytes = ms.ToArray();
                        bilder.Attachments.Add(file.FileName, filebytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            bilder.HtmlBody = body;
            email.Body = bilder.ToMessageBody();
            email.From.Add(new MailboxAddress(_emailSittings.DisplayName, _emailSittings.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSittings.Host, _emailSittings.port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSittings.Email, _emailSittings.password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
