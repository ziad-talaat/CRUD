using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{
    public class ApplicationDbContext:DbContext
    {
        
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasKey(x => x.Id);
            modelBuilder.Entity<City>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<City>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<City>().Property(x => x.Code).HasMaxLength(10);
            modelBuilder.Entity<City>().Property(x => x.Country).HasMaxLength(50);


        }
    }
}
