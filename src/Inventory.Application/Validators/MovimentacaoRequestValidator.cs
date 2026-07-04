using FluentValidation;
using Inventory.Application.DTOs.Requests;

namespace Inventory.Application.Validators;

public class MovimentacaoRequestValidator : AbstractValidator<MovimentacaoRequestDto>
{
    public MovimentacaoRequestValidator()
    {
        RuleFor(x => x.ProdutoId)
            .NotEmpty().WithMessage("O ID do produto é obrigatório.");

        RuleFor(x => x.Quantidade)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
    }
}