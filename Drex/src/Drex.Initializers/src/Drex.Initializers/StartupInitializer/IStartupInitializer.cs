namespace Drex.Initializers
{
    public interface IStartupInitializer : IInitializer
    {
        void AddInitializer(IInitializer initializer);
    }
}