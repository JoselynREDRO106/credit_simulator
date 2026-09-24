using CreditCatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditCatalogService.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
	public DbSet<CreditType> CreditTypes => Set<CreditType>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CreditType>(entity =>
		{
			entity.ToTable("credit_types");
			entity.HasKey(creditType => creditType.Id);
			entity.Property(creditType => creditType.Id).HasColumnName("id");
			entity.Property(creditType => creditType.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
			entity.Property(creditType => creditType.Descripcion).HasColumnName("descripcion");
			entity.Property(creditType => creditType.TasaAnual).HasColumnName("tasa_anual").HasPrecision(5, 2);
		});
	}
}
