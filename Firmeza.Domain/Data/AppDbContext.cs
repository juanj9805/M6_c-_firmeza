using Firmeza.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Domain.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleDetail> SaleDetails { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Sale>(entity =>
        {
            entity.HasOne(s => s.Client)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(s => s.SubTotal).HasPrecision(18, 2);
            entity.Property(s => s.Tax).HasPrecision(18, 2);
            entity.Property(s => s.Total).HasPrecision(18, 2);
        });

        builder.Entity<SaleDetail>(entity =>
        {
            entity.HasOne(sd => sd.Sale)
                .WithMany(s => s.SaleDetails)
                .HasForeignKey(s => s.SaleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(sd => sd.Product)
                .WithMany(p => p.SaleDetails)
                .HasForeignKey(sd => sd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(sd => sd.UnitPrice).HasPrecision(18, 2);
            entity.Property(sd => sd.Total).HasPrecision(18, 2);
            
        });
        
        builder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Price).HasPrecision(18, 2);
        });
    }
}