using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrinkStorage.Persistence.Brands;

internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("brands");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}
