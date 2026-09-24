namespace CreditCatalogService.Dtos;

public sealed record CreditTypeDto(
	int Id,
	string Nombre,
	string? Descripcion,
	decimal TasaAnual);
