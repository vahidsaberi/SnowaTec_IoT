using System.Threading.Tasks;
using SnowaTec.Test.Domain.Settings;

namespace SnowaTec.Test.Service.Contract.Possibility
{
    public interface ISmsService
    {
        Task<bool> SendSMSAsync(SmsRequest mailRequest);
    }
}
