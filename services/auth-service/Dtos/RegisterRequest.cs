using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos;

public sealed class RegisterRequest
{
	[Required, StringLength(100, MinimumLength = 2)]
	public string Nombre { get; set; } = string.Empty;

	[Required, EmailAddress, StringLength(150)]
	public string Email { get; set; } = string.Empty;

	[Required, MinLength(6), StringLength(100)]
	public string Password { get; set; } = string.Empty;
}
