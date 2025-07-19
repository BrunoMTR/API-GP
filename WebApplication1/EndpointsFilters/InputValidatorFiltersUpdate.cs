
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Presentation.EndpointsFilters
{
    public class InputValidatorFiltersUpdate<T> : IEndpointFilter
    {
        private readonly IValidator<T> _validator;

        public InputValidatorFiltersUpdate(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            T? inputData = context.GetArgument<T>(0);
            if (inputData is not null)
            {

                var validatorContext = new ValidationContext<T>(inputData);

                if (context.HttpContext.Request.RouteValues.TryGetValue("id", out var routeIdObj)
                    && int.TryParse(routeIdObj?.ToString(), out int routeId))
                {
                    validatorContext.RootContextData["RouteId"] = routeId;
                }

                var validationResult = await _validator.ValidateAsync(validatorContext);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

            }
            return await next.Invoke(context);
        }
    }
}
