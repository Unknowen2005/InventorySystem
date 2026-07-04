using System;

namespace Inventory.Application.DTOs.Requests;

public class MovimentacaoRequestDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public string Observacao { get; set; }
}