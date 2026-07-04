using System;
using System.Threading.Tasks;
using Inventory.Application.Common;
using Inventory.Application.DTOs.Requests;
using Inventory.Application.DTOs.Responses;
using Inventory.Application.Interfaces.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Interfaces.Repositories;

namespace Inventory.Application.Services;

public class EstoqueService : IEstoqueService
{
    private readonly IUnitOfWork _uow;

    public EstoqueService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ResultadoOperacao> RegistrarEntradaAsync(MovimentacaoRequestDto dto)
    {
        try
        {
            var produto = await _uow.ProdutoRepository.GetByIdAsync(dto.ProdutoId);
            if (produto == null)
                return ResultadoOperacao.Falha("Produto não encontrado.");

            produto.AdicionarEstoque(dto.Quantidade);

            var movimentacao = new Movimentacao(
                produto.id,
                Guid.Empty, // Será substituído pelo usuário logado depois
                TipoMovimentacao.Entrada,
                dto.Quantidade,
                dto.Observacao
            );

            await _uow.MovimentacaoRepository.AddAsync(movimentacao);
            await _uow.CommitAsync();

            return ResultadoOperacao.Ok("Entrada registrada com sucesso.");
        }
        catch (DomainException ex)
        {
            return ResultadoOperacao.Falha(ex.Message);
        }
        catch (Exception ex)
        {
            return ResultadoOperacao.Falha($"Erro ao registrar entrada: {ex.Message}");
        }
    }

    public async Task<ResultadoOperacao> RegistrarSaidaAsync(MovimentacaoRequestDto dto)
    {
        try
        {
            var produto = await _uow.ProdutoRepository.GetByIdAsync(dto.ProdutoId);
            if (produto == null)
                return ResultadoOperacao.Falha("Produto não encontrado.");

            produto.RemoverEstoque(dto.Quantidade);

            var movimentacao = new Movimentacao(
                produto.id,
                Guid.Empty, // Será substituído pelo usuário logado depois
                TipoMovimentacao.Saida,
                dto.Quantidade,
                dto.Observacao
            );

            await _uow.MovimentacaoRepository.AddAsync(movimentacao);
            await _uow.CommitAsync();

            return ResultadoOperacao.Ok("Saída registrada com sucesso.");
        }
        catch (DomainException ex)
        {
            return ResultadoOperacao.Falha(ex.Message);
        }
        catch (Exception ex)
        {
            return ResultadoOperacao.Falha($"Erro ao registrar saída: {ex.Message}");
        }
    }

    public async Task<ResultadoOperacao> ObterHistoricoAsync(Guid produtoId)
    {
        // Placeholder - implementar depois
        return ResultadoOperacao.Ok("Histórico obtido (placeholder).");
    }
}