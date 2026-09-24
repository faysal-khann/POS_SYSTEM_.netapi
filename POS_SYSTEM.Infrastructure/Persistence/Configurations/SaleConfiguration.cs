using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Infrastructure.Persistence.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(s => s.SaleId);

        builder.Property(s => s.InvoiceNo).IsRequired().HasMaxLength(30);
        builder.Property(s => s.PriceType).HasMaxLength(30);
        builder.Property(s => s.PaymentMethod).HasMaxLength(20);
        builder.Property(s => s.PaymentStatus).HasMaxLength(20);
        builder.Property(s => s.Status).HasMaxLength(20);
        builder.Property(s => s.ParkName).HasMaxLength(100);

        builder.Property(s => s.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(s => s.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.GrandTotal).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ReceivedAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ChangeAmount).HasColumnType("decimal(18,2)");

        builder.HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.SaleItems)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");
        builder.HasKey(i => i.SaleItemId);

        builder.Property(i => i.Qty).HasColumnType("decimal(18,2)");
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(i => i.DiscountPercent).HasColumnType("decimal(5,2)");
        builder.Property(i => i.TaxPercent).HasColumnType("decimal(5,2)");
        builder.Property(i => i.LineTotal).HasColumnType("decimal(18,2)");

        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}