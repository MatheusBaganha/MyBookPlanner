using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Repository.Context.Configurations
{
    public class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
    {
        public void Configure(EntityTypeBuilder<UserBook> builder)
        {
            builder.ToTable("UserBooks");

            builder.HasKey(ub => new { ub.IdUser, ub.IdBook });

            builder.Property(ub => ub.UserScore)
                   .IsRequired();

            builder.Property(ub => ub.ReadingStatus)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(ub => ub.User)
                   .WithMany(u => u.UserBooks)
                   .HasForeignKey(ub => ub.IdUser)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ub => ub.Book)
                   .WithMany(b => b.UserBooks)
                   .HasForeignKey(ub => ub.IdBook)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}