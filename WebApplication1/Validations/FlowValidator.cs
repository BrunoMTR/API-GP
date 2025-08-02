using FluentValidation;
using Presentation.Models;

namespace Presentation.Validations
{
    public class FlowValidator : AbstractValidator<Graph>
    {
        public FlowValidator()
        {
            RuleFor(g => g.Nodes)
           .NotNull().WithMessage("A lista de nós (nodes) não pode ser nula.")
           .NotEmpty().WithMessage("Deve existir pelo menos 2 nós (nodes) no grafo.")
           .Must(n => n.Count >= 2 && n.Count <= 10)
               .WithMessage("O grafo deve conter no mínimo 2 e no máximo 10 nós (nodes).");

            RuleForEach(g => g.Nodes).SetValidator(new NodesValidator());
        }
    }
}
