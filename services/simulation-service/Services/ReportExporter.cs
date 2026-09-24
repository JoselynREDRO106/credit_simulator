using System.Globalization;
using System.Text;
using simulation_service.Models;

namespace simulation_service.Services;

public sealed class ReportExporter
{
	public byte[] Export(Simulation simulation)
	{
		var builder = new StringBuilder();
		builder.AppendLine("numero_cuota,cuota,interes,abono_capital,saldo");
		foreach (var installment in simulation.Installments.OrderBy(item => item.NumeroCuota))
		{
			builder.AppendLine(string.Join(",",
				installment.NumeroCuota,
				Format(installment.Cuota),
				Format(installment.Interes),
				Format(installment.AbonoCapital),
				Format(installment.Saldo)));
		}

		return Encoding.UTF8.GetBytes(builder.ToString());
	}

	private static string Format(decimal value) =>
		value.ToString("0.00", CultureInfo.InvariantCulture);
}
