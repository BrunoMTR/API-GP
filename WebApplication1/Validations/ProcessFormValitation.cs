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
          
        }
    }
}
