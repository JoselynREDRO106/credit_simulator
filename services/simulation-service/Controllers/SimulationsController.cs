using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simulation_service.Calculators;
using simulation_service.Clients;
using simulation_service.Data;
using simulation_service.Dtos;
using simulation_service.Models;
using simulation_service.Services;

namespace simulation_service.Controllers;

[ApiController]
[Authorize]
[Route("api/simulations")]
public class SimulationsController(
	SimulationDbContext dbContext,
	CreditCatalogClient creditCatalogClient,
	FrenchCalculator frenchCalculator,
	GermanCalculator germanCalculator,
	ReportExporter reportExporter) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult<SimulationDto>> Create(SimulationRequest request, CancellationToken cancellationToken)
	{
		var userId = GetUserId();
		if (userId is null)
		{
			return Unauthorized();
		}

		var creditType = await creditCatalogClient.GetByIdAsync(request.CreditTypeId, cancellationToken);
		if (creditType is null)
		{
			return BadRequest(new { message = "El tipo de crédito no existe." });
		}

		var method = request.Metodo.ToUpperInvariant();
		IAmortizationCalculator calculator = method == "ALEMAN" ? germanCalculator : frenchCalculator;
		var installments = calculator.Calculate(request.Monto, request.PlazoMeses, creditType.TasaAnual);
		var simulation = new Simulation
		{
			UsuarioId = userId.Value,
			CreditTypeId = creditType.Id,
			TipoCredito = creditType.Nombre,
			Monto = request.Monto,
			PlazoMeses = request.PlazoMeses,
			TasaAnual = creditType.TasaAnual,
			Metodo = method,
			TotalIntereses = installments.Sum(installment => installment.Interes),
			TotalPagado = installments.Sum(installment => installment.Cuota),
			CreatedAt = DateTime.UtcNow,
			Installments = installments.ToList()
		};

		dbContext.Simulations.Add(simulation);
		await dbContext.SaveChangesAsync(cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id = simulation.Id }, ToDto(simulation));
	}

	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<SimulationDto>>> GetHistory(CancellationToken cancellationToken)
	{
		var userId = GetUserId();
		if (userId is null)
		{
			return Unauthorized();
		}

		var simulations = await dbContext.Simulations
			.AsNoTracking()
			.Include(simulation => simulation.Installments)
			.Where(simulation => simulation.UsuarioId == userId.Value)
			.OrderByDescending(simulation => simulation.CreatedAt)
			.ToListAsync(cancellationToken);

		return Ok(simulations.Select(ToDto).ToList());
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<SimulationDto>> GetById(int id, CancellationToken cancellationToken)
	{
		var userId = GetUserId();
		if (userId is null)
		{
			return Unauthorized();
		}

		var simulation = await dbContext.Simulations
			.AsNoTracking()
			.Include(item => item.Installments)
			.SingleOrDefaultAsync(item => item.Id == id && item.UsuarioId == userId.Value, cancellationToken);

		return simulation is null ? NotFound() : Ok(ToDto(simulation));
	}

	[HttpGet("{id:int}/export")]
	public async Task<IActionResult> Export(int id, CancellationToken cancellationToken)
	{
		var userId = GetUserId();
		if (userId is null)
		{
			return Unauthorized();
		}

		var simulation = await dbContext.Simulations
			.AsNoTracking()
			.Include(item => item.Installments)
			.SingleOrDefaultAsync(item => item.Id == id && item.UsuarioId == userId.Value, cancellationToken);

		return simulation is null
			? NotFound()
			: File(reportExporter.Export(simulation), "text/csv", $"simulation-{id}.csv");
	}

	private int? GetUserId()
	{
		var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
			?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
		return int.TryParse(value, out var userId) ? userId : null;
	}

	private static SimulationDto ToDto(Simulation simulation) => new(
		simulation.Id,
		simulation.UsuarioId,
		simulation.CreditTypeId,
		simulation.TipoCredito,
		simulation.Monto,
		simulation.PlazoMeses,
		simulation.TasaAnual,
		simulation.Metodo,
		simulation.TotalIntereses,
		simulation.TotalPagado,
		simulation.CreatedAt,
		simulation.Installments
			.OrderBy(installment => installment.NumeroCuota)
			.Select(installment => new InstallmentDto(
				installment.NumeroCuota,
				installment.Cuota,
				installment.Interes,
				installment.AbonoCapital,
				installment.Saldo))
			.ToList());
}
