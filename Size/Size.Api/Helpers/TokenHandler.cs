using Microsoft.IdentityModel.Tokens;
using Size.Core.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Size.Api.Helpers
{
    public static class TokenHandler
    {
        public static string BuildJWTToken(ApiSettings apiSettings)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = apiSettings.Issuer;
            var audience = apiSettings.Audience;
            var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(apiSettings.TokenExpiry));

            var token = new JwtSecurityToken(issuer,
              audience,
              expires: jwtValidity,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
