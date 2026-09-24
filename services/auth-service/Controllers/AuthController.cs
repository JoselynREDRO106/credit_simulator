using AuthService.Data;
using AuthService.Dtos;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthDbContext dbContext, JwtTokenService jwtTokenService) : ControllerBase
{
	[HttpPost("register")]
	public async Task<ActionResult<AuthResponse>> Register(
		RegisterRequest request,
		CancellationToken cancellationToken)
	{
		var email = request.Email.Trim().ToLowerInvariant();
		if (await dbContext.Users.AnyAsync(user => user.Email == email, cancellationToken))
		{
			return Conflict(new { message = "El correo ya está registrado." });
		}

		var user = new User
		{
			Nombre = request.Nombre.Trim(),
			Email = email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
			CreatedAt = DateTime.UtcNow
		};

		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync(cancellationToken);

		return StatusCode(StatusCodes.Status201Created, CreateResponse(user));
	}

	[HttpPost("login")]
	public async Task<ActionResult<AuthResponse>> Login(
		LoginRequest request,
		CancellationToken cancellationToken)
	{
		var email = request.Email.Trim().ToLowerInvariant();
		var user = await dbContext.Users.SingleOrDefaultAsync(
			candidate => candidate.Email == email,
			cancellationToken);

		if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
		{
			return Unauthorized(new { message = "Correo o contraseña incorrectos." });
		}

		return Ok(CreateResponse(user));
	}

	private AuthResponse CreateResponse(User user) =>
		new(jwtTokenService.CreateToken(user), user.Id, user.Nombre, user.Email);
}
