using Microsoft.EntityFrameworkCore;
using Taxi.Web.Data.Entities;

namespace Taxi.Web.Data
{
    public class DataContext : DbContext
    {

        //TODO Make a Snippets of this ctor and DbSet<>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<TaxiEntity> Taxis { get; set; }
        public DbSet<Entities.TripDetailEntity> TripDetais { get; set; }
        public DbSet<Entities.TripEntity> Trips { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaxiEntity>()
                .HasIndex(t => t.Plaque)
                .IsUnique();
        }

    }
}
