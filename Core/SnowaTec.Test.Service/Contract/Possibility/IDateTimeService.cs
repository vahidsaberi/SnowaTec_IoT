namespace SnowaTec.Test.Service.Contract.Possibility
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }

        DateTime Now { get; }
    }
}
