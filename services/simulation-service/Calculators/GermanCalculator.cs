using simulation_service.Models;

namespace simulation_service.Calculators;

public class GermanCalculator : IAmortizationCalculator
{
	public IReadOnlyList<Installment> Calculate(decimal principal, int termMonths, decimal annualRate)
	{
		if (principal <= 0 || termMonths <= 0 || annualRate < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(principal), "Los valores del crédito no son válidos.");
		}

		var monthlyRate = annualRate / 100m / 12m;
		var capitalPayment = Round(principal / termMonths);
		var installments = new List<Installment>(termMonths);
		var balance = principal;

		for (var month = 1; month <= termMonths; month++)
		{
			var interest = Round(balance * monthlyRate);
			var capital = month == termMonths ? balance : capitalPayment;
			var amount = Round(capital + interest);
			balance = Round(balance - capital);
			installments.Add(new Installment
			{
				NumeroCuota = month,
				Cuota = amount,
				Interes = interest,
				AbonoCapital = capital,
				Saldo = balance
			});
		}

		return installments;
	}

	private static decimal Round(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
