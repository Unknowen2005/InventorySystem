using AutoMapper;
using Inventory.Application.DTOs.Responses;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Application.Mappings;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Produto, ProdutoResponseDto>()
            .ForMember(dest => dest.CategoriaNome,
                opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : null))
            .ForMember(dest => dest.QuantidadeEmEstoque,
                opt => opt.MapFrom(src => src.QuantidadeEmEstoque));

        CreateMap<Movimentacao, MovimentacaoResponseDto>()
            .ForMember(dest => dest.NomeProduto,
                opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : null))
            .ForMember(dest => dest.Tipo,
                opt => opt.MapFrom(src => src.Tipo == TipoMovimentacao.Entrada ? "Entrada" : "Saída"));
    }
}