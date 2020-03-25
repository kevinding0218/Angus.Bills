using Drex.Initializers;
using Microsoft.Extensions.DependencyInjection;

namespace Drex.Auth.Distributed
{
    public static class Extensions
    {
        private const string RegistryName = "auth.distributed";

        public static IDrexBuilder AddDistributedAccessTokenValidator(this IDrexBuilder builder)
        {
            if (!builder.TryRegister(RegistryName)) return builder;

            builder.Services.AddSingleton<IAccessTokenService, DistributedAccessTokenService>();

            return builder;
        }
    }
}