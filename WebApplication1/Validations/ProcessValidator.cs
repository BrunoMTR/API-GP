using FluentValidation;
using Presentation.Models.Form;
using System.Text;
using System.Text.RegularExpressions;


namespace Presentation
{
    public class ProcessValidator : AbstractValidator<ProcessForm>
    {
        public ProcessValidator()
        {
            RuleFor(x => x.Notes)
            .MaximumLength(50)
            .WithMessage("{PropertyName} must be 50 characters or fewer.")
            .Must(desc => Encoding.UTF8.GetByteCount(desc) == desc.Length)
            .WithMessage("Description contains unsupported characters.")
            .Custom((notes, context) =>
            {
                Regex rg = new Regex("<.*>");
                if (rg.Matches(notes).Count > 0)
                {
                    context.AddFailure(
                        new FluentValidation.Results.ValidationFailure(
                            "Notes", "the parameter has invalid context."));
                }
            });

            RuleFor(p => p.ProcessCode)
           .NotEmpty().WithMessage("Process Code is required.")
           .Matches(@"^[A-Z]{2,10}-\d{4}-\d+$")
           .WithMessage("ProcessCode must follow the format: APPLICATIONACRONYM-YYYY-ID.");




            RuleFor(p => p.CreatedById)
                .GreaterThan(0)
                .WithMessage("Creation date id is required.");



            RuleFor(p => p.ApplicationId)
               .GreaterThan(0)
               .WithMessage("Application id is required.");


        }
        
    }
}
