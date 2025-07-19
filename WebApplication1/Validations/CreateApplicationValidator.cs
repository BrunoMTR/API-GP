using Domain.Repositories;
using FluentValidation;
using Presentation.Models;
using Presentation.Models.Requests;

namespace Presentation.Validations
{
    public class CreateApplicationValidator : ApplicationValidatorBase<CreateApplication>
    {
        public CreateApplicationValidator(IApplicationRepository repository) : base()
        {
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellation) =>
                {
                    return !await repository.ExistsByName(name);
                })
                .WithMessage("The Name is already in use.");

            RuleFor(x => x.Abbreviation)
                .MustAsync(async (abbreviation, cancellation) =>
                {
                    return !await repository.ExistsByAbbreviation(abbreviation);
                })
                .WithMessage("The Abbreviation is already in use.");
        }
    }
}
