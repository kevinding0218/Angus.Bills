namespace Drex.LoadBalancing.Fabio
{
    public interface IFabioOptionsBuilder
    {
        IFabioOptionsBuilder Enable(bool enabled);
        IFabioOptionsBuilder WithUrl(string url);
        IFabioOptionsBuilder WithService(string service);
        FabioOptions Build();
    }
}