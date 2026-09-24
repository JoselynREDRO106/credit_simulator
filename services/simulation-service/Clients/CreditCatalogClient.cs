using System.Net.Http.Json;

namespace simulation_service.Clients;

public sealed class CreditCatalogClient(HttpClient httpClient)
{
	public async Task<CreditTypeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		var creditTypes = await httpClient.GetFromJsonAsync<List<CreditTypeResponse>>(
			"api/credit-types", cancellationToken);
		return creditTypes?.SingleOrDefault(creditType => creditType.Id == id);
	}
}

public sealed record CreditTypeResponse(
	int Id,
	string Nombre,
	string? Descripcion,
	decimal TasaAnual);
