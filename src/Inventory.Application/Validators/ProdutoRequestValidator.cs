using FluentValidation;
using Inventory.Application.DTOs.Requests;

namespace Inventory.Application.Validators;

public class ProdutoRequestValidator : AbstractValidator<ProdutoRequestDto>
{
    public ProdutoRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Preco)
            .GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo.");

        RuleFor(x => x.QuantidadeInicial)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade inicial não pode ser negativa.");
    }
}