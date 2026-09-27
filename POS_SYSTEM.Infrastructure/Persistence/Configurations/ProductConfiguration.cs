using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.ProductCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.ProductCode).IsUnique();

        builder.Property(p => p.ProductName).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Barcode).HasMaxLength(100);
        builder.Property(p => p.ImageUrl).HasMaxLength(500);
        builder.Property(p => p.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Active");
        builder.Property(p => p.Description).HasMaxLength(1000);

        builder.Property(p => p.PurchasePrice).HasColumnType("decimal(10,2)");
        builder.Property(p => p.SalePrice).HasColumnType("decimal(10,2)");
        builder.Property(p => p.TaxPercent).HasColumnType("decimal(5,2)");

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Unit)
            .WithMany()
            .HasForeignKey(p => p.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}