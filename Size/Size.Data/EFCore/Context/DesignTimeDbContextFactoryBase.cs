using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Size.Data.EFCore.Helpers;
using System;

namespace Size.Data.EFCore.Context
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> :
      IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            return Create();
        }
        protected abstract TContext CreateNewInstance(
            DbContextOptions<TContext> options);


        private TContext Create()
        {
            var connstr = ConnectionHelper.GetConnectionString();

            if (string.IsNullOrWhiteSpace(connstr))
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'Size'.");
            }
            return Create(connstr);
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
             $"{nameof(connectionString)} is null or empty.",
             nameof(connectionString));

            var optionsBuilder =
                 new DbContextOptionsBuilder<TContext>();

            Console.WriteLine(
                "MyDesignTimeDbContextFactory.Create(string): Connection string: {0}",
                connectionString);

            optionsBuilder.UseNpgsql(connectionString, o => o.SetPostgresVersion(9, 6));

            var options = optionsBuilder.Options;
            return CreateNewInstance(options);
        }
    }
}
