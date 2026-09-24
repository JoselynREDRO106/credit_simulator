using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
	public DbSet<User> Users => Set<User>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("users");
			entity.HasKey(user => user.Id);
			entity.Property(user => user.Id).HasColumnName("id");
			entity.Property(user => user.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
			entity.Property(user => user.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
			entity.Property(user => user.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
			entity.Property(user => user.CreatedAt).HasColumnName("created_at").IsRequired();
			entity.HasIndex(user => user.Email).IsUnique();
		});
	}
}
