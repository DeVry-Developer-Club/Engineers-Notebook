namespace EngineerNotebook.PublicApi.Services;
using EngineerNotebook.GrpcContracts.Authentication;
using EngineerNotebook.Shared.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class AuthenticationService : Authentication.AuthenticationBase
{
    private readonly ITokenClaimsService _tokenClaimsService;
    private readonly SignInManager<ClubMember> _signInManager;

    public AuthenticationService(SignInManager<ClubMember> signInManager, ITokenClaimsService tokenClaimsService)
    {
        _signInManager = signInManager;
        _tokenClaimsService = tokenClaimsService;
    }

    public override async Task<RegistrationResponse> Register(RegistrationRequest request, ServerCallContext context)
    {
        if (request.Password != request.ConfirmPassword)
            return new RegistrationResponse { Success = false };

        ClubMember user = new ClubMember
        {
            Email = request.Username,
            UserName = request.Username,
            NormalizedEmail = request.Username.ToUpper(),
            NormalizedUserName = request.Username.ToUpper()
        };

        var result = await _signInManager.UserManager.CreateAsync(user, request.Password);

        return new RegistrationResponse { Success = result.Succeeded };
    }

    public override async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, ServerCallContext context)
    {
        var response = new AuthenticationResponse();

        Console.WriteLine(request.Username);
        Console.WriteLine(request.Password);
        Console.WriteLine(_signInManager is null);

        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

        response.Result = result.Succeeded;
        response.IsLockedOut = result.IsLockedOut;
        response.RequiresTwoFactor = result.RequiresTwoFactor;
        response.Username = request.Username;

        if (result.Succeeded)
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);

        return response;
    }
}
