using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CodePusle.API.Repositories.Implementation
{
	public class TokenRespository : ITokenRespository
	{
		private readonly IConfiguration configuration;

		public TokenRespository(IConfiguration configuration)
        {
			this.configuration = configuration;
		}

        public string CreateJWTToken(IdentityUser user, List<string> roles)
		{
			// Create a claims

			var claims = new List<Claim>();

			claims.Add(new Claim(ClaimTypes.Email, user.Email));

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			// Create Security Token Parameter

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: configuration["Jwt:Issuer"],
				audience: configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(15),
				signingCredentials: credentials);

			// Return token

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
