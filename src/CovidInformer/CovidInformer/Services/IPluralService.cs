namespace CovidInformer.Services
{
    public interface IPluralService
    {
        string GetQuantityString(string groupId, ulong quantity, params object[] args);
    }
}