using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Persistence.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchases");
            builder.HasKey(p => p.PurchaseId);

            builder.Property(p => p.PurchaseNo).IsRequired().HasMaxLength(30);
            builder.HasIndex(p => p.PurchaseNo).IsUnique();

            builder.Property(p => p.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(p => p.DiscountAmount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.TaxPercent).HasColumnType("decimal(5,2)");
            builder.Property(p => p.TaxAmount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.ShippingCharge).HasColumnType("decimal(18,2)");
            builder.Property(p => p.GrandTotal).HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.PurchaseItems)
                .WithOne(i => i.Purchase)
                .HasForeignKey(i => i.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> builder)
        {
            builder.ToTable("PurchaseItems");
            builder.HasKey(i => i.PurchaseItemId);

            builder.Property(i => i.Qty).HasColumnType("decimal(18,2)");
            builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(i => i.DiscountPercent).HasColumnType("decimal(5,2)");
            builder.Property(i => i.LineTotal).HasColumnType("decimal(18,2)");

            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Size)
                .WithMany()
                .HasForeignKey(i => i.SizeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
