using Application.Common.Settings;
using Domain.Constants;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.Login.Commands.LoginJamaagent
{
    public record LoginJamaaAgentResponse(bool success, string token, string message)
    {
        public static LoginJamaaAgentResponse Success(string token)
            => new LoginJamaaAgentResponse(true, token, "authorized successfully");
        public static LoginJamaaAgentResponse Fail(string errorMess = "Invalid credentials")
            => new LoginJamaaAgentResponse(false, null, errorMess);
    }
    public record LoginJamaaAgentCommand : IRequest<LoginJamaaAgentResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    internal sealed class LoginJamaaAgentCommandHandler : IRequestHandler<LoginJamaaAgentCommand, LoginJamaaAgentResponse>
    {
        private readonly AuthSetting _authSetting;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginJamaaAgentCommandHandler(AuthSetting authSetting, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _authSetting = authSetting;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginJamaaAgentResponse> Handle(LoginJamaaAgentCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                return LoginJamaaAgentResponse.Fail();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

                if (signInResult.Succeeded)
                {
                    var token = GenerateJwtToken(user);

                    return LoginJamaaAgentResponse.Success(token);
                }
            }

            return LoginJamaaAgentResponse.Fail();
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var keyBytes = new byte[64];
            Encoding.UTF8.GetBytes(_authSetting.SecretKey).CopyTo(keyBytes, 0);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                _authSetting.Issuer,
                _authSetting.Audience,
                claims: CreateJamaaAgentClaimsFor(user),
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private IEnumerable<Claim> CreateJamaaAgentClaimsFor(ApplicationUser user)
           => new[] {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, Roles.JamaaAgent),
                    new Claim(JamaaHubClaims.UniversityId, user.UniversityId.ToString()),
               };
    }
}
