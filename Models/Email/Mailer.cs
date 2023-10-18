
using System.Threading.Tasks;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using System;
using MimeKit.Text;

namespace smartlocker.software.api.Models.Email
{
    public class Mailer : IMailer
    {
        public async Task SendEmailAsync(string email, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Smartlocker", "none@none.com"));
                message.To.Add(new MailboxAddress("Not Reply", email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = htmlBody
                };
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("infosmartlocker@gmail.com", "20SmartLocker21!");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(e.Message);
            }
        }
    }
}