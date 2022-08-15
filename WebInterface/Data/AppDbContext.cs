using Microsoft.EntityFrameworkCore;
using WebInterface.Models;

namespace WebInterface.Data
{
    /// <summary>
    /// Class for connection with database.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
    }
}
