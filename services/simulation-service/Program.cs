using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using simulation_service.Calculators;
using simulation_service.Clients;
using simulation_service.Data;
using simulation_service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
	policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SimulationDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("SimulationDatabase")));
builder.Services.AddScoped<FrenchCalculator>();
builder.Services.AddScoped<GermanCalculator>();
builder.Services.AddScoped<ReportExporter>();
builder.Services.AddHttpClient<CreditCatalogClient>(client =>
{
	var baseUrl = builder.Configuration["Services:CreditCatalogUrl"]
		?? throw new InvalidOperationException("Services:CreditCatalogUrl is not configured.");
	client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
});

var jwtKey = builder.Configuration["Jwt:Key"]
	?? throw new InvalidOperationException("Jwt:Key is not configured.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
			ValidateIssuer = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidateAudience = true,
			ValidAudience = builder.Configuration["Jwt:Audience"],
			ValidateLifetime = true,
			ClockSkew = TimeSpan.FromMinutes(1)
		};
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();
