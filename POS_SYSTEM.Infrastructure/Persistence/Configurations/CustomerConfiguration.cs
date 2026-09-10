using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.CustomerCode).IsRequired().HasMaxLength(20);
            builder.HasIndex(c => c.CustomerCode).IsUnique();

            builder.Property(c => c.CustomerName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Phone).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Email).HasMaxLength(100);
            builder.Property(c => c.CustomerGroup).IsRequired().HasMaxLength(50);
            builder.Property(c => c.NationalIdTaxId).HasMaxLength(50);

            builder.Property(c => c.AddressLine1).IsRequired().HasMaxLength(200);
            builder.Property(c => c.AddressLine2).HasMaxLength(200);
            builder.Property(c => c.City).IsRequired().HasMaxLength(50);
            builder.Property(c => c.StateDivision).HasMaxLength(100);
            builder.Property(c => c.PostalCode).HasMaxLength(20);
            builder.Property(c => c.Country).HasMaxLength(100).HasDefaultValue("Bangladesh");

            builder.Property(c => c.OpeningBalance).HasColumnType("decimal(12,2)");
            builder.Property(c => c.CreditLimit).HasColumnType("decimal(12,2)");
            builder.Property(c => c.DueAmount).HasColumnType("decimal(12,2)");

            builder.Property(c => c.Status).HasMaxLength(20).HasDefaultValue("Active");
        }
    }
}
