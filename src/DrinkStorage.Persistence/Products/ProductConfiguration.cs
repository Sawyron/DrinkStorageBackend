using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrinkStorage.Persistence.Products;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(p => p.Price)
            .IsRequired();
        builder.Property(p => p.Quantity)
            .IsRequired();
        builder.Property(p => p.ImageUrl)
            .IsRequired();
        builder.HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .IsRequired();
    }
}
