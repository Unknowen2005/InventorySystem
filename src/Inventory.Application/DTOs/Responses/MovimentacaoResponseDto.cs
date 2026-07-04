using System;

namespace Inventory.Application.DTOs.Responses;

public class MovimentacaoResponseDto
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; }
    public string Tipo { get; set; } // "Entrada" ou "Saída"
    public int Quantidade { get; set; }
    public DateTime DataHora { get; set; }
    public string Observacao { get; set; }
}