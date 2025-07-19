using FluentValidation;
using Presentation.Models;
using System.Text.RegularExpressions;

namespace Presentation.Validations
{
    public class ApplicationValidatorBase<T> : AbstractValidator<T> where T : Application
    {
        public ApplicationValidatorBase()
        {
            RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage("Name is required")
              .MaximumLength(20)
              .WithMessage("Name length must be 15 caracteres or fewer")
             .Custom((name, context) =>
             {
                 if (!string.IsNullOrEmpty(name))
                 {
                     Regex rg = new Regex("<.*>");
                     if (rg.Matches(name).Count > 0)
                     {
                         context.AddFailure("Name", "The parameter has invalid content.");
                     }
                 }
             });

            RuleFor(x => x.TeamEmail)
                .EmailAddress()
                .WithMessage("Email address must be valid");

            RuleFor(x => x.ApplicationEmail)
                .EmailAddress()
                .WithMessage("Email address must be valid");

            RuleFor(x => x.Abbreviation)
                .MaximumLength(10)
                .WithMessage("Abbreviation must have 10 caracteres or fewer");

            RuleFor(x => x.Team)
                .MaximumLength(20)
                .WithMessage("Team must have 20 caracteres or fewer");
        }
    }
}
