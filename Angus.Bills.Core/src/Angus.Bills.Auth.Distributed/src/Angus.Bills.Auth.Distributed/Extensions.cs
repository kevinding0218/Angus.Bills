using Angus.Bills.Initializers;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.Auth.Distributed
{
    public static class Extensions
    {
        private const string RegistryName = "auth.distributed";

        public static IAngusBillsBuilder AddDistributedAccessTokenValidator(this IAngusBillsBuilder builder)
        {
            if (!builder.TryRegister(RegistryName)) return builder;

            builder.Services.AddSingleton<IAccessTokenService, DistributedAccessTokenService>();

            return builder;
        }
    }
}