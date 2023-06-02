namespace WorkflowsPayments.Services
{
    public interface ISystemClock
    {
        DateTime UtcNow { get; }
    }
}