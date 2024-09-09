using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrinkStorage.Persistence.Coins;

internal class CoinConfiguration : IEntityTypeConfiguration<Coin>
{
    public void Configure(EntityTypeBuilder<Coin> builder)
    {
        builder.ToTable("coins");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Value)
            .IsRequired();
        builder.HasIndex(c => c.Value)
            .IsUnique();
        builder.Property(c => c.Count)
            .IsRequired();
    }
}
