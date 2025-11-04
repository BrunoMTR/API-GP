using Domain.DTOs;
using Domain.Results;
using Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.Request;
using System.Security.Claims;

namespace Presentation.Endpoints
{
    public class UserEndpoints
    {

        public static async Task<IResult>PostUserLogout(
            [FromServices] IAuthService authService,
            HttpContext httpContext
            )
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok(new Response<string> { Success = true, Data = "User logged out successfully." });
        }
        public static async Task<IResult> PostUserLogin(
            [FromBody] LoginRequest request,
            [FromServices] IAuthService authService,
            HttpContext httpContext
            )
        {
            var user = await authService.AuthenticateUserAsync(request.Username, request.Password);
            if (user == null)
            {
                return Results.Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("LastChanged", user.LastChanged.ToString("O"))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity),
               new AuthenticationProperties
               {
                   IsPersistent = true,
                   ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
               });

            return Results.Ok(new Response<UserDto> { Success = true, Message = "User authenticated successfully.", Data = user });

        }
    }
}
