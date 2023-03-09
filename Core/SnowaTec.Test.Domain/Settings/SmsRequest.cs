using System.Collections.Generic;

namespace SnowaTec.Test.Domain.Settings
{
    public class SmsRequest
    {
        public int TemplateId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
