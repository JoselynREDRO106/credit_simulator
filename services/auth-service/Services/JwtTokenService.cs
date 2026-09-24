using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services;

public class JwtTokenService(IConfiguration configuration)
{
	public string CreateToken(User user)
	{
		var key = configuration["Jwt:Key"]
			?? throw new InvalidOperationException("Jwt:Key is not configured.");
		var credentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
			SecurityAlgorithms.HmacSha256);
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.Nombre),
			new Claim(ClaimTypes.Email, user.Email)
		};
		var expirationMinutes = configuration.GetValue<int?>("Jwt:ExpirationMinutes") ?? 60;
		var token = new JwtSecurityToken(
			issuer: configuration["Jwt:Issuer"],
			audience: configuration["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
			signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
