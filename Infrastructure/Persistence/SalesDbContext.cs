using Microsoft.EntityFrameworkCore;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Infrastructure.Persistence;

public class SalesDbContext : DbContext
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            // Value Objects
            entity.OwnsOne(u => u.Name, n =>
            {
                n.Property(x => x.Firstname).IsRequired().HasColumnName("Firstname");
                n.Property(x => x.Lastname).IsRequired().HasColumnName("Lastname");
            });

            entity.OwnsOne(u => u.Address, a =>
            {
                a.Property(x => x.City).IsRequired().HasColumnName("City");
                a.Property(x => x.Street).IsRequired().HasColumnName("Street");
                a.Property(x => x.Number).IsRequired().HasColumnName("Number");
                a.Property(x => x.Zipcode).IsRequired().HasColumnName("Zipcode");

                a.OwnsOne(x => x.Geolocation, g =>
                {
                    g.Property(p => p.Lat!).IsRequired().HasColumnName("Geo_Lat");
                    g.Property(p => p.Long!).IsRequired().HasColumnName("Geo_Long");
                });
            });

            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.Property(e => e.Role)
                .HasConversion<string>();
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasMany(c => c.Products)
                .WithOne()
                .HasForeignKey("CartId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity => { entity.HasKey(ci => new { ci.ProductId, ci.Quantity }); });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey("SaleId")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}