using FluentValidation;
using Presentation.Models;
using System.Text;
using System.Text.RegularExpressions;


namespace Presentation
{
    public class ProcessValidator : AbstractValidator<Process>
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


            RuleFor(p => p.CreatedAt)
                .NotEmpty().WithMessage("Creation date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Data of creation cannot be in the future.");

            RuleFor(p => p.LastUpdatedAt)
                .GreaterThanOrEqualTo(p => p.CreatedAt)
                .When(p => p.LastUpdatedAt.HasValue)
                .WithMessage("Date of lat update must be later than or equal todate of creation.");


            RuleFor(p => p.StateId)
                .GreaterThan(0)
                .WithMessage("A valid State must be selected.");

            RuleFor(p => p.CreatedById)
                .GreaterThan(0)
                .WithMessage("Creation date id is required.");



            RuleFor(p => p.ApplicationId)
               .GreaterThan(0)
               .WithMessage("Application id is required.");


            RuleFor(p => p.HolderId)
                .GreaterThan(0)
                .WithMessage("Holder id is required.");
        }
        
    }
}
