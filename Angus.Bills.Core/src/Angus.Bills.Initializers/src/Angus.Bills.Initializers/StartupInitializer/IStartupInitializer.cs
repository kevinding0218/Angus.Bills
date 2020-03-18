namespace Angus.Bills.Initializers {
    public interface IStartupInitializer : IInitializer {
        void AddInitializer (IInitializer initializer);
    }
}