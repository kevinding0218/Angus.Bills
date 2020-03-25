using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Drex.Initializers
{
    public class DrexBuilder : IDrexBuilder
    {
        private readonly List<Action<IServiceProvider>> _buildActions;
        private readonly ConcurrentDictionary<string, bool> _registry = new ConcurrentDictionary<string, bool>();
        private readonly IServiceCollection _services;

        private DrexBuilder(IServiceCollection services)
        {
            _buildActions = new List<Action<IServiceProvider>>();
            _services = services;
            _services.AddSingleton<IStartupInitializer>(new StartupInitializer());
        }

        IServiceCollection IDrexBuilder.Services => _services;

        public bool TryRegister(string name)
        {
            return _registry.TryAdd(name, true);
        }

        public void AddBuildAction(Action<IServiceProvider> execute)
        {
            _buildActions.Add(execute);
        }

        public void AddInitializer(IInitializer initializer)
        {
            AddBuildAction(sp =>
            {
                var startupInitializer = sp.GetService<IStartupInitializer>();
                startupInitializer.AddInitializer(initializer);
            });
        }

        public void AddInitializer<TInitializer>() where TInitializer : IInitializer
        {
            AddBuildAction(sp =>
            {
                var initializer = sp.GetService<TInitializer>();
                var startupInitializer = sp.GetService<IStartupInitializer>();
                startupInitializer.AddInitializer(initializer);
            });
        }

        public IServiceProvider Build()
        {
            var serviceProvider = _services.BuildServiceProvider();
            _buildActions.ForEach(a => a(serviceProvider));
            return serviceProvider;
        }

        public static IDrexBuilder Create(IServiceCollection services)
        {
            return new DrexBuilder(services);
        }
    }
}