namespace PostOffice.Repository.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    
    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server with connection string from app settings
        options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"), b => b.MigrationsAssembly("PostOffice.Api"));
    }

    public DbSet<Parcel> Parcels { get; set; }
    public DbSet<Bag> Bags { get; set; }
    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parcel>()
            .HasOne<Bag>(p => p.Bag)
            .WithMany(b => b.Parcels)
            .HasForeignKey(p => p.BagId);

        modelBuilder.Entity<Bag>()
           .HasOne<Shipment>(p => p.Shipment)
           .WithMany(b => b.Bags)
           .HasForeignKey(p => p.ShipmentId);

        modelBuilder.Entity<Shipment>()
           .HasAlternateKey(c => c.ShipmentNumber)
           .HasName("AlternateKey_ShipmentNumber");

        modelBuilder.Entity<Bag>()
           .HasAlternateKey(c => c.BagNumber)
           .HasName("AlternateKey_BagNumber");

        modelBuilder.Entity<Parcel>()
           .HasAlternateKey(c => c.ParcelNumber)
           .HasName("AlternateKey_ParcelNumber");

    }
}
