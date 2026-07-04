using AutoMapper;
using Inventory.Application.DTOs.Responses;
using Inventory.Domain.Entities;

namespace Inventory.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Produto, ProdutoResponseDto>()
            .ForMember(dest => dest.CategoriaNome,
                opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : null));
    }
}