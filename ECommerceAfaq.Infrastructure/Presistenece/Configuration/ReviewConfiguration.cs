using ECommerceAfaq.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Presistenece.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(r => r.UserFullName).IsRequired().HasMaxLength(150);

            builder.Property(r=>r.Comment).HasMaxLength(1000);

            builder.HasOne(r=>r.Product)
                .WithMany(p=>p.Reviews)
                .HasForeignKey(r=>r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(w => new { w.UserId, w.ProductId }).IsUnique(); // Composite Unique Index

            builder.ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5"));


        }
    }
}
