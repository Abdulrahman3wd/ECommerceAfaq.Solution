using ECommerceAfaq.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Presistenece.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);
            builder.Property(o => o.ShippingFullname).IsRequired().HasMaxLength(150);
            builder.Property(o => o.ShippingPhone).IsRequired().HasMaxLength(20);
            builder.Property(o => o.ShippingArea).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ShippingCity).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ShippingStreet).IsRequired().HasMaxLength(200);

            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

            builder.Property(o => o.OrderStatus).HasConversion<string>().HasMaxLength(20);
            builder.Property(o => o.PaymentMethod).HasConversion<string>().HasMaxLength(20);
            builder.Property(o => o.PaymentStatus).HasConversion<string>().HasMaxLength(20);


            builder.HasMany(o=>o.OrderItems)
                .WithOne(o=>o.Order)
                .HasForeignKey(oi=>oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.UserId);



        }
    }
}
