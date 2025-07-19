using FluentValidation;

namespace Presentation.EndpointsFilters
{
    public class InputValidatorFilter<T> : IEndpointFilter
    {
        private readonly IValidator<T> _valitator;
        public InputValidatorFilter(IValidator<T> valitator)
        {
            _valitator = valitator;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            T? inputData = context.GetArgument<T>(0);
            if (inputData is not null)
            {
                var validationResult = await _valitator.ValidateAsync(inputData);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

            }
            return await next.Invoke(context);
        }
    }
}
