using System;

namespace Inventory.Application.DTOs.Requests;

public class ProdutoRequestDto
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeInicial { get; set; }
    public Guid? CategoriaId { get; set; }
    public string LocalizacaoPrateleira { get; set; }
}