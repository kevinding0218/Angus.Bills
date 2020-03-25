namespace Angus.Bills.Persistence.SqlServer
{
    public interface ISqlServerOptionsBuilder
    {
        ISqlServerOptionsBuilder WithConnectionString(string connectionString, bool inMemory);
        ISqlServerOptionsBuilder WithDatabase(string database, bool inMemory);
        SqlServerOptions Build();
    }
}