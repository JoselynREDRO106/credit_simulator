namespace simulation_service.Dtos;

public sealed record InstallmentDto(
	int NumeroCuota,
	decimal Cuota,
	decimal Interes,
	decimal AbonoCapital,
	decimal Saldo);
