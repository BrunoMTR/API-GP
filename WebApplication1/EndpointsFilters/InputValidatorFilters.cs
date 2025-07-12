
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Presentation.EndpointsFilters
{
    public class InputValidatorFilters<T> : IEndpointFilter
    {
        private readonly IValidator<T> _validator;

        public InputValidatorFilters(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            T? inputData = context.GetArgument<T>(0);
            if (inputData is not null)
            {
                var validationResult = await _validator.ValidateAsync(inputData);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

            }
            return await next.Invoke(context);
        }
    }
}
