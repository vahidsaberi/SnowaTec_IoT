using SnowaTec.Test.Domain.Settings;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Contract.Possibility
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}
