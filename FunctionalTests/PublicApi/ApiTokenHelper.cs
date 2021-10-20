using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EngineerNotebook.Core.Constants;
using Microsoft.IdentityModel.Tokens;

namespace FunctionalTests.PublicApi
{
    public class ApiTokenHelper
    {
        static string userName = "admin@ddc.org";
        static string id = "test-id";
        static string[] roles = { "Administrators" };
        
        public static string GetAdminUserToken() => CreateToken(id, userName, roles);

        private static string CreateToken(string id, string userName, string[] roles)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
            
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}