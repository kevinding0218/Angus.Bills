namespace Angus.Bills.Persistence.SqlServer
{
    public class SqlServerOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool InMemory { get; set; }
        public bool Seed { get; set; }
    }
}