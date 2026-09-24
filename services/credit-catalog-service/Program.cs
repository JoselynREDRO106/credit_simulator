using CreditCatalogService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
	policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("CatalogDatabase")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

app.Run();
