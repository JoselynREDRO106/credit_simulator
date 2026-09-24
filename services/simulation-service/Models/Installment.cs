namespace simulation_service.Models;

public class Installment
{
	public int Id { get; set; }

	public int SimulationId { get; set; }

	public int NumeroCuota { get; set; }

	public decimal Cuota { get; set; }

	public decimal Interes { get; set; }

	public decimal AbonoCapital { get; set; }

	public decimal Saldo { get; set; }

	public Simulation? Simulation { get; set; }
}
