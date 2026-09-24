using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Infrastructure.Persistence.Configurations;

public class SaleReturnConfiguration : IEntityTypeConfiguration<SaleReturn>
{
    public void Configure(EntityTypeBuilder<SaleReturn> builder)
    {
        builder.ToTable("SaleReturns");
        builder.HasKey(sr => sr.SaleReturnId);

        builder.Property(sr => sr.ReturnType).HasMaxLength(20).HasDefaultValue("Sales Return");
        builder.Property(sr => sr.Reason).HasMaxLength(50);
        builder.Property(sr => sr.Note).HasMaxLength(500);
        builder.Property(sr => sr.RefundMethod).HasMaxLength(20);

        builder.Property(sr => sr.SubTotal).HasPrecision(18, 2);
        builder.Property(sr => sr.TaxAmount).HasPrecision(18, 2);
        builder.Property(sr => sr.GrandTotal).HasPrecision(18, 2);
        builder.Property(sr => sr.ReceivedAmount).HasPrecision(18, 2);

        builder.HasMany(sr => sr.SaleReturnItems)
               .WithOne(i => i.SaleReturn)
               .HasForeignKey(i => i.SaleReturnId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SaleReturnItemConfiguration : IEntityTypeConfiguration<SaleReturnItem>
{
    public void Configure(EntityTypeBuilder<SaleReturnItem> builder)
    {
        builder.ToTable("SaleReturnItems");
        builder.HasKey(sri => sri.SaleReturnItemId);

        builder.Property(sri => sri.ReturnQty).HasPrecision(18, 2);
        builder.Property(sri => sri.UnitPrice).HasPrecision(18, 2);
        builder.Property(sri => sri.LineTotal).HasPrecision(18, 2);

        builder.HasOne(sri => sri.Product)
               .WithMany()
               .HasForeignKey(sri => sri.ProductId);
    }
}