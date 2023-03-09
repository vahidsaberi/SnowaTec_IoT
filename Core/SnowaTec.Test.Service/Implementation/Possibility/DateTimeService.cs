using SnowaTec.Test.Service.Contract.Possibility;

namespace SnowaTec.Test.Service.Implementation.Possibility
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;
    }
}
