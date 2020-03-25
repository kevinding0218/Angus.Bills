using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Angus.Bills.Persistence.SqlServer.Initializers
{
    public abstract class SqlServerInitializer : ISqlServerInitializer
    {
        private readonly ILogger<SqlServerInitializer> _logger;
        private readonly IOptions<SqlServerOptions> _sqlServerOptions;

        public SqlServerInitializer(
            IOptions<SqlServerOptions> sqlServerOptions,
            ILogger<SqlServerInitializer> logger)
        {
            _sqlServerOptions = sqlServerOptions;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            await SeedData();
            _logger.LogInformation("SqlServerInitializer.");
        }

        // Let child Initilizaer override this method
        protected abstract Task SeedData();
    }
}