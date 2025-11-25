namespace FirearmTracker.Core.Interfaces
{
    public interface IHealthCheckService
    {
        Task<HealthCheckResults> RunChecksAsync();
    }
}