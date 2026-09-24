namespace simulation_service.Dtos;

public sealed record SimulationDto(
	int Id,
	int UsuarioId,
	int CreditTypeId,
	string TipoCredito,
	decimal Monto,
	int PlazoMeses,
	decimal TasaAnual,
	string Metodo,
	decimal TotalIntereses,
	decimal TotalPagado,
	DateTime CreatedAt,
	IReadOnlyList<InstallmentDto> Installments);
