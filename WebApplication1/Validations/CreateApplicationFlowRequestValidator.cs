using FluentValidation;
using Presentation.Models.Forms;


namespace Presentation.Validations
{
    public class CreateApplicationFlowRequestValidator : AbstractValidator<CreateApplicationFlowRequest>
    {
        public CreateApplicationFlowRequestValidator()
        {
            RuleFor(x => x.Application)
                .NotNull().WithMessage("A aplicação é obrigatória.")
                .SetValidator(new ApplicationValidator());

            RuleFor(x => x.Graph)
                .NotNull().WithMessage("O grafo é obrigatório.")
                .SetValidator(new FlowValidator());
        }
    }
}
