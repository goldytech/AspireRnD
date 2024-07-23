using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQLApi.Data.Entities;

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>

{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Product");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(p => p.CategoryEntity)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        builder.Property(p => p.RowVersion).IsRowVersion();

        builder.Property(p => p.LastModifiedAt)
            .ValueGeneratedOnAddOrUpdate();
    }
}