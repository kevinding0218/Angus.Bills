namespace Drex.Persistence.SqlServer.Builders
{
    internal sealed class SqlServerOptionsBuilder : ISqlServerOptionsBuilder
    {
        private readonly SqlServerOptions _options = new SqlServerOptions();

        public ISqlServerOptionsBuilder WithConnectionString(string connectionString, bool inMemory = false)
        {
            _options.ConnectionString = connectionString;
            _options.InMemory = inMemory;
            return this;
        }

        public ISqlServerOptionsBuilder WithDatabase(string database, bool inMemory = false)
        {
            _options.Database = database;
            _options.InMemory = inMemory;
            return this;
        }

        public SqlServerOptions Build()
        {
            return _options;
        }
    }
}