namespace CreditCatalogService.Models;

public class CreditType
{
	public int Id { get; set; }

	public string Nombre { get; set; } = string.Empty;

	public string? Descripcion { get; set; }

	public decimal TasaAnual { get; set; }
}
