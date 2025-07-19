using Domain.Repositories;
using FluentValidation;
using Presentation.Models;
using Presentation.Models.Requests;

namespace Presentation.Validations
{
    public class UpdateApplicationValidator : ApplicationValidatorBase<UpdateApplication>
    {
        public UpdateApplicationValidator(IApplicationRepository repository) : base()
        {
            RuleFor(x => x.Name)
        .MustAsync(async (application, name, context, cancellation) =>
        {
            if (!context.RootContextData.TryGetValue("RouteId", out var routeIdObj) || routeIdObj is not int routeId)
                return false;

            var exists = await repository.ExistsByNameExceptId(name, routeId);
            return !exists;
        }).WithMessage("The Name is already in use by another application.");

            RuleFor(x => x.Abbreviation)
                .MustAsync(async (application, abbreviation, context, cancellation) =>
                {
                    if (!context.RootContextData.TryGetValue("RouteId", out var routeIdObj) || routeIdObj is not int routeId)
                        return false;

                    var exists = await repository.ExistsByAbbreviationExceptId(abbreviation, routeId);
                    return !exists;
                }).WithMessage("The Abbreviation is already in use by another application.");
        }
    }
}
