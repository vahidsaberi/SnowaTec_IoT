using Newtonsoft.Json;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Domain.Helper;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.DTO.Recovery
{
    public class BackupDto : BaseEntity
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public long Key { get; set; }
        public string Data { get; set; }

        [NotMapped]
        public string PersianCreateRowDate
        {
            get
            {
                return PersianDateHelper.EnglishToPersianDate(CreateRowDate);
            }
        }

        public ActionType ActionType { get; set; }

        [NotMapped]
        public string ActionTypeValue { get { return ActionTypeFunction.GetValue(ActionType); } }

        public T Value<T>()
        {
            return JsonConvert.DeserializeObject<T>(Data);
        }

        public long UserId { get; set; }

        public string UserName { get; set; }
    }
}
