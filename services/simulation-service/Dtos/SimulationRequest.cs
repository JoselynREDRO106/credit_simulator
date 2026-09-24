using System.ComponentModel.DataAnnotations;

namespace simulation_service.Dtos;

public sealed class SimulationRequest
{
	[Range(1, 5)]
	public int CreditTypeId { get; set; }

	[Range(typeof(decimal), "0.01", "999999999999")]
	public decimal Monto { get; set; }

	[Range(1, 600)]
	public int PlazoMeses { get; set; }

	[Required, RegularExpression("FRANCES|ALEMAN")]
	public string Metodo { get; set; } = "FRANCES";
}
