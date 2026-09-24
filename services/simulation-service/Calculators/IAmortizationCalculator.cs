using simulation_service.Models;

namespace simulation_service.Calculators;

public interface IAmortizationCalculator
{
	IReadOnlyList<Installment> Calculate(decimal principal, int termMonths, decimal annualRate);
}
