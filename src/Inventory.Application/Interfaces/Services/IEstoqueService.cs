using System.Threading.Tasks;
using Inventory.Application.DTOs.Requests;
using Inventory.Application.DTOs.Responses;
using Inventory.Application.Common;

namespace Inventory.Application.Interfaces.Services;

public interface IEstoqueService
{
    Task<ResultadoOperacao> RegistrarEntradaAsync(MovimentacaoRequestDto dto);
    Task<ResultadoOperacao> RegistrarSaidaAsync(MovimentacaoRequestDto dto);
    Task<ResultadoOperacao> ObterHistoricoAsync(Guid produtoId);
}