using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class FilesMonitorDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<SourceFile> SourceFiles { get; set; }

        public FilesMonitorDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SourceFile>().HasIndex(s => new { s.Path }).IsUnique();
        }
    }
}
