using CreditCatalogService.Data;
using CreditCatalogService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditCatalogService.Controllers;

[ApiController]
[Route("api/credit-types")]
public class CreditTypesController(CatalogDbContext dbContext) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<CreditTypeDto>>> GetAll(CancellationToken cancellationToken)
	{
		var creditTypes = await dbContext.CreditTypes
			.AsNoTracking()
			.OrderBy(creditType => creditType.Id)
			.Select(creditType => new CreditTypeDto(
				creditType.Id,
				creditType.Nombre,
				creditType.Descripcion,
				creditType.TasaAnual))
			.ToListAsync(cancellationToken);

		return Ok(creditTypes);
	}
}
