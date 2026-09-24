using Microsoft.EntityFrameworkCore;
using simulation_service.Models;

namespace simulation_service.Data;

public class SimulationDbContext(DbContextOptions<SimulationDbContext> options) : DbContext(options)
{
	public DbSet<Simulation> Simulations => Set<Simulation>();

	public DbSet<Installment> Installments => Set<Installment>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Simulation>(entity =>
		{
			entity.ToTable("simulations");
			entity.HasKey(simulation => simulation.Id);
			entity.Property(simulation => simulation.Id).HasColumnName("id");
			entity.Property(simulation => simulation.UsuarioId).HasColumnName("usuario_id");
			entity.Property(simulation => simulation.CreditTypeId).HasColumnName("credit_type_id");
			entity.Property(simulation => simulation.TipoCredito).HasColumnName("tipo_credito").HasMaxLength(50);
			entity.Property(simulation => simulation.Monto).HasColumnName("monto").HasPrecision(15, 2);
			entity.Property(simulation => simulation.PlazoMeses).HasColumnName("plazo_meses");
			entity.Property(simulation => simulation.TasaAnual).HasColumnName("tasa_anual").HasPrecision(5, 2);
			entity.Property(simulation => simulation.Metodo).HasColumnName("metodo").HasMaxLength(20);
			entity.Property(simulation => simulation.TotalIntereses).HasColumnName("total_intereses").HasPrecision(15, 2);
			entity.Property(simulation => simulation.TotalPagado).HasColumnName("total_pagado").HasPrecision(15, 2);
			entity.Property(simulation => simulation.CreatedAt).HasColumnName("created_at");
			entity.HasMany(simulation => simulation.Installments)
				.WithOne(installment => installment.Simulation)
				.HasForeignKey(installment => installment.SimulationId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<Installment>(entity =>
		{
			entity.ToTable("installments");
			entity.HasKey(installment => installment.Id);
			entity.Property(installment => installment.Id).HasColumnName("id");
			entity.Property(installment => installment.SimulationId).HasColumnName("simulation_id");
			entity.Property(installment => installment.NumeroCuota).HasColumnName("numero_cuota");
			entity.Property(installment => installment.Cuota).HasColumnName("cuota").HasPrecision(15, 2);
			entity.Property(installment => installment.Interes).HasColumnName("interes").HasPrecision(15, 2);
			entity.Property(installment => installment.AbonoCapital).HasColumnName("abono_capital").HasPrecision(15, 2);
			entity.Property(installment => installment.Saldo).HasColumnName("saldo").HasPrecision(15, 2);
		});
	}
}
