using System.Threading.Tasks;

namespace smartlocker.software.api.Models.Email
{
    public interface IMailer
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}