using Presentation.Endpoints;

namespace Presentation.RouteGroups
{
    public static class UserGroup
    {
        public static void AddUserGroup(this WebApplication app)
        {
            var versionSet = app.NewApiVersionSet().HasApiVersion(new Asp.Versioning.ApiVersion(1, 0)).Build();
            var group = app.MapGroup("users").WithTags("Users").WithApiVersionSet(versionSet).HasApiVersion(1.0);
            

            group.MapPost("/login", UserEndpoints.PostUserLogin)
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "User login",
                    Description = "Authenticates a user with provided credentials."
                });

            group.MapPost("/logout", UserEndpoints.PostUserLogout)
                .DisableAntiforgery()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "User logout",
                    Description = "Logs out the currently authenticated user."
                });

        }
    }
}
