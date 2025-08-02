using FluentValidation;
using Presentation.Models;

namespace Presentation.Validations
{
    public class NodesValidator : AbstractValidator<Node>
    {
        public NodesValidator()
        {
            RuleFor(n => n.OriginId).GreaterThan(0);
            RuleFor(n => n.DestinationId).GreaterThan(0);
            When(n => n.Approvals.HasValue, () =>
            {
                RuleFor(n => n.Approvals.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Approvals must be a non-negative number.");
            });
            
        }
    }
}
