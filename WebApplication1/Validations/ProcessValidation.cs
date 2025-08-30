using FluentValidation;
using Presentation.Models;
using System.Text.RegularExpressions;

namespace Presentation.Validations
{
    public class ProcessValidation : AbstractValidator<Process>
    {
        public ProcessValidation()
        {
            RuleFor(p => p.ApplicationId)
                .GreaterThan(0)
                .WithMessage("ApplicationId must be greater than 0.");

            RuleFor(p => p.CreatedBy)
                .NotEmpty()
                .WithMessage("CreatedBy is required.")
                .MaximumLength(100)
                .WithMessage("CreatedBy cannot exceed 100 characters.");

        }
    }
}
