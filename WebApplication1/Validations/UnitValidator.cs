using FluentValidation;
using Presentation.Models;
using System.Text.RegularExpressions;

namespace Presentation.Validations
{
    public class UnitValidator : AbstractValidator<Unit>
    {
        public UnitValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.")
                .Custom((name, context) =>
                {
                    Regex rg = new Regex("<.*?>");
                    if (!string.IsNullOrEmpty(name) && rg.IsMatch(name))
                    {
                        context.AddFailure(
                            new FluentValidation.Results.ValidationFailure(
                                "Name", "O campo contém HTML inválido.")
                        );
                    }
                });

            RuleFor(u => u.Abbreviation)
                .MaximumLength(10).WithMessage("A sigla deve ter no máximo 10 caracteres.")
                .Custom((abbreviation, context) =>
                {
                    Regex rg = new Regex("<.*?>");
                    if (!string.IsNullOrEmpty(abbreviation) && rg.IsMatch(abbreviation))
                    {
                        context.AddFailure(
                            new FluentValidation.Results.ValidationFailure(
                                "Abbreviation", "O campo contém HTML inválido.")
                        );
                    }
                });

            RuleFor(u => u.Email)
                .EmailAddress().When(u => !string.IsNullOrWhiteSpace(u.Email))
                .WithMessage("O email deve ser válido.");
        }
    }
}
