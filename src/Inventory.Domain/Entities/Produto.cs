using Inventory.Domain.Exceptions;
using Inventory.Shared;

namespace Inventory.Domain.Entities;

public class Produto : BaseEntity
{
    public string Nome { get; private set; }
    public decimal Preco { get; private set; }
    public int QuantidadeEmEstoque { get; private set; }
    public Guid? CategoriaId { get; private set; }
    public string? LocalizacaoPrateleira { get; private set; }

    // Navegação (opcional para o EF Core)
    private Produto() { }
    public virtual Categoria? Categoria { get; private set; }

    public Produto(string nome, decimal preco, int quantidadeInicial, Guid? categoriaId = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do produto é obrigatório.");
        if (preco < 0)
            throw new DomainException("O preço não pode ser negativo.");

        Nome = nome;
        Preco = preco;
        CategoriaId = categoriaId;

        // Reusa a regra de adição para garantir validação
        AdicionarEstoque(quantidadeInicial);
    }

    public void AdicionarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade a adicionar deve ser positiva.");

        QuantidadeEmEstoque += quantidade;
        DataModificacao = DateTime.Now;
    }

    public void RemoverEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade a remover deve ser positiva.");

        if (QuantidadeEmEstoque - quantidade < 0)
            throw new DomainException($"Estoque insuficiente. Disponível: {QuantidadeEmEstoque}, Solicitado: {quantidade}.");

        QuantidadeEmEstoque -= quantidade;
        DataModificacao = DateTime.Now;
    }

    public void AtualizarDados(string nome, decimal preco, string? localizacao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do produto é obrigatório.");
        if (preco < 0)
            throw new DomainException("O preço não pode ser negativo.");

        Nome = nome;
        Preco = preco;
        LocalizacaoPrateleira = localizacao;
        DataModificacao = DateTime.Now;
    }

    public void AssociarCategoria(Categoria categoria)
    {
        Categoria = categoria ?? throw new DomainException("Categoria inválida.");
        CategoriaId = categoria.id;
    }
}