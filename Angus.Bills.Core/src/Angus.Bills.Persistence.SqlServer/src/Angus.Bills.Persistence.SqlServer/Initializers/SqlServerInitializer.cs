using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Angus.Bills.Persistence.SqlServer.Initializers
{
    public abstract class SqlServerInitializer : ISqlServerInitializer
    {
        private readonly IOptions<SqlServerOptions> _sqlServerOptions;
        private readonly ILogger<SqlServerInitializer> _logger;

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
            return;
        }

        // Let child Initilizaer override this method
        protected abstract Task SeedData();
    }
}