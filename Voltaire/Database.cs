using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Voltaire
{
    public partial class DataBase : DbContext
    {
        public DbSet<Models.Guild> Guilds { get; set; }
        public DbSet<Models.BannedIdentifier> BannedIdentifiers { get; set; }
                private string _connection;

        public DataBase()
        {
        }

        public DataBase(string connection) : base()
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql($@"{_connection}", x => x.ServerVersion("10.4.17-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataBase>
    {
        public DataBase CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            return new DataBase(configuration.GetConnectionString("sql"));
        }
    }
}
