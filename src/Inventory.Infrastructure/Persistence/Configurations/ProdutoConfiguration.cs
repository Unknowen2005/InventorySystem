using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(p => p.id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Preco)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.QuantidadeEmEstoque)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.LocalizacaoPrateleira)
            .HasMaxLength(50);

        builder.HasIndex(p => p.Nome)
            .IsUnique();

        // Relacionamento com Categoria
        builder.HasOne(p => p.Categoria)
            .WithMany()
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}