using FluentValidation;
using Presentation.Models.Forms;

namespace Presentation.Validations
{
    public class ProcessDocumentationValidation : AbstractValidator<DocumentForm>
    {
        public ProcessDocumentationValidation()
        {
            RuleFor(p => p.ProcessId)
                .GreaterThan(0)
                .WithMessage("ApplicationId must be greater than 0.");

            RuleFor(p => p.UploadedBy)
                .NotEmpty()
                .WithMessage("CreatedBy is required.")
                .MaximumLength(100)
                .WithMessage("CreatedBy cannot exceed 100 characters.");
        }
    }
}
