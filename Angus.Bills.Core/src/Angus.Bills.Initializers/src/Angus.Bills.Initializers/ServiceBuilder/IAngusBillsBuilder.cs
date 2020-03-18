using System;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.Initializers {
    public interface IAngusBillsBuilder {
        IServiceCollection Services { get; }
        bool TryRegister (string name);
        void AddBuildAction (Action<IServiceProvider> execute);
        void AddInitializer (IInitializer initializer);
        void AddInitializer<TInitializer> () where TInitializer : IInitializer;
        IServiceProvider Build ();
    }
}