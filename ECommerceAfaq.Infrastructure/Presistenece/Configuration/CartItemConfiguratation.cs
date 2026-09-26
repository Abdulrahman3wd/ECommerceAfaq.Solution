using ECommerceAfaq.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Presistenece.Configuration
{
    public class CartItemConfiguratation : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(ci => ci.Id);

            builder.Property(c => c.Quantity)
                .IsRequired();

            builder.HasOne(ci=>ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
