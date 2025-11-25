namespace FirearmTracker.Core.Interfaces
{
    public interface ICaliberService
    {
        List<string> GetAllCalibers();
        void AddCustomCaliber(string caliber);
    }
}
