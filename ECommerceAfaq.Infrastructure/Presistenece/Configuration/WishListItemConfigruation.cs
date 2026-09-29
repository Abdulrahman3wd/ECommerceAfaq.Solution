using ECommerceAfaq.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Presistenece.Configuration
{
    public class WishListItemConfigruation : IEntityTypeConfiguration<WishListItem>
    {
        public void Configure(EntityTypeBuilder<WishListItem> builder)
        {
            builder.ToTable("WishListItems"); 
            builder.HasKey(w => w.Id);

            builder.Property(w => w.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.HasOne(w=>w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(w => new { w.UserId , w.ProductId}).IsUnique(); // Composite Unique Index
        }
    }
}
