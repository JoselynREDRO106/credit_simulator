namespace simulation_service.Models;

public class Simulation
{
	public int Id { get; set; }

	public int UsuarioId { get; set; }

	public int CreditTypeId { get; set; }

	public string TipoCredito { get; set; } = string.Empty;

	public decimal Monto { get; set; }

	public int PlazoMeses { get; set; }

	public decimal TasaAnual { get; set; }

	public string Metodo { get; set; } = string.Empty;

	public decimal TotalIntereses { get; set; }

	public decimal TotalPagado { get; set; }

	public DateTime CreatedAt { get; set; }

	public List<Installment> Installments { get; set; } = [];
}
