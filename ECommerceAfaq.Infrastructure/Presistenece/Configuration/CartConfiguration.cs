using ECommerceAfaq.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Presistenece.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts"); 
            builder.HasKey(c=>c.Id);

            builder.Property(c => c.UserId)
                .IsRequired()
                .HasMaxLength(450); 

            // One cart has many CartItems  

            builder.HasMany(c=>c.CartItems)
                .WithOne(ci=>ci.Cart)
                .HasForeignKey(ci=>ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(c => c.UserId).IsUnique(); 



        }
    }
}
