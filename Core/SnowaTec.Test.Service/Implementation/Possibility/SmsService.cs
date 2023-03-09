using Aqua.EnumerableExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsIrRestfulNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Settings;
using SnowaTec.Test.Service.Contract.Possibility;
using SnowaTec.Test.Service.Exceptions;

namespace SnowaTec.Test.Service.Implementation.Possibility
{
    public class SmsService : ISmsService
    {
        public SmsSettings _smsSettings { get; }
        public ILogger<SmsService> _logger { get; }

        public SmsService(IOptions<SmsSettings> smsSettings, ILogger<SmsService> logger)
        {
            _smsSettings = smsSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendSMSAsync(SmsRequest smsRequest)
        {
            try
            {
                var token = new Token().GetToken(_smsSettings.ApiKey, _smsSettings.SecretKey);
                UltraFastSendRespone ultraFastSendRespone = new UltraFastSendRespone();

                var parameterArray = new List<UltraFastParameters>();

                smsRequest.Parameters.ForEach(x =>
                {
                    parameterArray.Add(new UltraFastParameters
                    {
                        Parameter = x.Key,
                        ParameterValue = x.Value
                    });
                });

                smsRequest.PhoneNumbers.ForEach(x =>
                {
                    var ultraFastSend = new UltraFastSend()
                    {
                        Mobile = Convert.ToInt64(x),
                        TemplateId = smsRequest.TemplateId,
                        ParameterArray = parameterArray.ToArray()
                    };

                    ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
                });

                return ultraFastSendRespone.IsSuccessful;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }
    }
}
