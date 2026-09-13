using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Persistence.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");
            builder.HasKey(s => s.SupplierId);

            builder.Property(s => s.SupplierCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(s => s.SupplierCode).IsUnique();

            builder.Property(s => s.SupplierName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Phone).HasMaxLength(20);
            builder.Property(s => s.Email).HasMaxLength(100);
            builder.Property(s => s.Website).HasMaxLength(150);

            builder.Property(s => s.AddressLine1).HasMaxLength(200);
            builder.Property(s => s.AddressLine2).HasMaxLength(200);
            builder.Property(s => s.City).HasMaxLength(50);
            builder.Property(s => s.StateDivision).HasMaxLength(100);
            builder.Property(s => s.PostalCode).HasMaxLength(20);
            builder.Property(s => s.Country).HasMaxLength(100).HasDefaultValue("Bangladesh");

            builder.Property(s => s.ContactPerson).HasMaxLength(150);
            builder.Property(s => s.ContactPersonPhone).HasMaxLength(20);
            builder.Property(s => s.TaxVatNo).HasMaxLength(50);

            builder.Property(s => s.OpeningBalance).HasColumnType("decimal(12,2)");
            builder.Property(s => s.CreditLimit).HasColumnType("decimal(12,2)");
            builder.Property(s => s.DueAmount).HasColumnType("decimal(12,2)");

            builder.Property(s => s.Status).HasMaxLength(20).HasDefaultValue("Active");
        }
    }
}
