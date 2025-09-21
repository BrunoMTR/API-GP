using FluentValidation;
using Presentation.Models.Forms;

namespace Presentation.Validations
{
    public class ProcessFormValitation: AbstractValidator<ProcessForm>
    {
        public ProcessFormValitation()
        {
            RuleFor(x => x.ApplicationId)
              .GreaterThan(0)
              .WithMessage("ProcessId must be greater than 0");
            RuleFor(x => x.CreatedBy)
                .NotEmpty()
                .WithMessage("UploadedBy is required")
                .MaximumLength(50)
                .WithMessage("UploadedBy length must be 50 caracteres or fewer");
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required")
                .Must(file => file != null && file.Length > 0)
                .WithMessage("File must not be empty");
        }
    }
}
