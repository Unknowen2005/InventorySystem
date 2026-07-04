using System;

namespace Inventory.Application.DTOs.Responses;

public class ProdutoResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEmEstoque { get; set; }
    public string CategoriaNome { get; set; }
    public string LocalizacaoPrateleira { get; set; }
}