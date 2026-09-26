using ECommerceAfaq.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Presistenece.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses"); 
            builder.HasKey(x => x.Id);
            builder.Property(a => a.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(a => a.FullName)
        .IsRequired()
        .HasMaxLength(150);
            builder.Property(a => a.PhoneNumber)
        .IsRequired()
        .HasMaxLength(20);
            builder.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(a => a.Area)
        .IsRequired()
        .HasMaxLength(100);
            builder.Property(a => a.Street)
        .IsRequired()
        .HasMaxLength(100);

            builder.HasIndex(a => a.UserId);
        }
    }
}
