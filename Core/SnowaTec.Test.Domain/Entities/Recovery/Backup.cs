using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Domain.Entities.Recovery
{
    public class Backup : BaseEntity
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public long Key { get; set; }
        public string Data { get; set; }
        public ActionType ActionType { get; set; }
        public long UserId { get; set; }
    }
}
