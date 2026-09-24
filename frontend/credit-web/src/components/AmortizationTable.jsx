export default function AmortizationTable({ installments = [], preview = false }) {
	const visibleInstallments = preview ? installments.slice(0, 3) : installments
	const remainingInstallments = installments.length - visibleInstallments.length

	return <div className={`table-wrap${preview ? ' table-preview' : ''}`}>
		<table><thead><tr><th>Cuota</th><th>Pago</th><th>Interés</th><th>Capital</th><th>Saldo</th></tr></thead><tbody>{visibleInstallments.map((item) => <tr key={item.numeroCuota}><td>{item.numeroCuota}</td><td>${item.cuota.toFixed(2)}</td><td>${item.interes.toFixed(2)}</td><td>${item.abonoCapital.toFixed(2)}</td><td>${item.saldo.toFixed(2)}</td></tr>)}</tbody></table>
		{preview && remainingInstallments > 0 && <p className="table-note">Mostrando las primeras 3 cuotas de {installments.length}. Consulta el detalle para ver toda la tabla.</p>}
	</div>
}
