using System;
using System.Linq;
using Angus.Bills.Initializers;
using Angus.Bills.Persistence.SqlServer.Builders;
using Angus.Bills.Persistence.SqlServer.Initializers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.Persistence.SqlServer
{
    public static class Extensions
    {
        private const string SectionName = "sql-server";
        private const string RegistryName = "persistence.sql.server";

        public static IAngusBillsBuilder AddSqlServer<TMyDbContext>(this IAngusBillsBuilder builder,
            string sectionName = SectionName)
            where TMyDbContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var mongoOptions = builder.GetOptions<SqlServerOptions>(sectionName);
            return builder.AddSqlServer<TMyDbContext>(mongoOptions);
        }

        public static IAngusBillsBuilder AddSqlServer<TMyDbContext>(this IAngusBillsBuilder builder,
            Func<ISqlServerOptionsBuilder,
                ISqlServerOptionsBuilder> buildOptions) where TMyDbContext : DbContext
        {
            var mongoOptions = buildOptions(new SqlServerOptionsBuilder()).Build();
            return builder.AddSqlServer<TMyDbContext>(mongoOptions);
        }

        public static IAngusBillsBuilder AddSqlServer<TMyDbContext>(this IAngusBillsBuilder builder,
            SqlServerOptions sqlServerOptions)
            where TMyDbContext : DbContext
        {
            if (!builder.TryRegister(RegistryName)) return builder;

            builder.Services.AddSingleton(sqlServerOptions);

            builder.Services.AddTransient<ISqlServerInitializer, SqlServerInitializer>();

            builder.AddInitializer<ISqlServerInitializer>();
            builder.Services.AddEntityFrameworkMsSql<TMyDbContext>(sqlServerOptions);

            return builder;
        }

        private static IServiceCollection AddEntityFrameworkMsSql<TMyDbContext>(this IServiceCollection service,
            SqlServerOptions sqlServerOptions)
            where TMyDbContext : DbContext
        {
            return service.AddEntityFrameworkInMemoryDatabase()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<TMyDbContext>((serviceProvider, options) =>
                    options.UseSqlServer(sqlServerOptions.ConnectionString)
                        .UseInternalServiceProvider(serviceProvider));
        }

        /// <summary>
        ///     Executes the specified raw SQL command and returns an integer specifying the number of rows affected by the SQL
        ///     statement passed to it
        /// </summary>
        /// <param name="dbContext">current db context</param>
        /// <param name="sql">
        ///     The raw SQL e.g: EXEC AddCategory @CategoryName/"INSERT Categories (CategoryName) VALUES
        ///     (@CategoryName)"
        /// </param>
        /// <param name="parameters">The SqlParameter.</param>
        /// <returns>The number of state entities written to database.</returns>
        public static int ExecuteSqlCommand(this DbContext dbContext, string sql, params SqlParameter[] parameters)
        {
            return dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        ///     Uses raw SQL queries to fetch the specified "TEntity" data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public static IQueryable<T> FromSql<T>(this DbContext dbContext, string sql, params SqlParameter[] parameters)
            where T : class
        {
            return dbContext.Set<T>().FromSqlRaw(sql, parameters);
        }
    }
}