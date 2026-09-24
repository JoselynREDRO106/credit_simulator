namespace AuthService.Dtos;

public sealed record AuthResponse(
	string Token,
	int UserId,
	string Nombre,
	string Email);
