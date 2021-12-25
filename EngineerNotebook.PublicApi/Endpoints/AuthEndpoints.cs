using EngineerNotebook.Shared.Endpoints.Auth;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EngineerNotebook.PublicApi.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/auth",
            async ([FromBody] LoginRequest request, [FromServices] SignInManager<ClubMember> signInManager, [FromServices] ITokenClaimsService tokenClaimsService) =>
            {
                if (request is null)
                    return Results.BadRequest();
            
                var response = new LoginResponse();
                var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

                response.Success = result.Succeeded;
                response.IsLockedOut = result.IsLockedOut;
                response.RequiresTwoFactor = result.RequiresTwoFactor;
                response.Email = request.Email;

                if (result.Succeeded)
                    response.Token = await tokenClaimsService.GetTokenAsync(request.Email);

                return Results.Ok(response);
            });
        
        return endpoints;
    }
}