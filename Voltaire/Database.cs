using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
                optionsBuilder.UseMySql("server=127.0.0.1;database=voltairedb;user id=voltaire;password=aJDAK05zHd8cPAuN", x => x.ServerVersion("10.4.17-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
