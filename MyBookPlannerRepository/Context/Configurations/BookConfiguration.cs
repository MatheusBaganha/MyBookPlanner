using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Repository.Context.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.Author)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.ReleaseYear)
                   .IsRequired();

            builder.Property(b => b.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(b => b.Score)
                   .IsRequired();
        }
    }
}
