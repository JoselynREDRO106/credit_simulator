import apiClient from './apiClient'
import { jsPDF } from 'jspdf'

const simulationApi = apiClient.create ? apiClient : apiClient
simulationApi.defaults.baseURL = import.meta.env.VITE_SIMULATION_API_URL || 'http://localhost:5003/api'

export const createSimulation = (data) => simulationApi.post('/simulations', data)
export const getSimulations = () => simulationApi.get('/simulations')
export const getSimulation = (id) => simulationApi.get(`/simulations/${id}`)
export const downloadSimulation = (id) => simulationApi.get(`/simulations/${id}/export`, { responseType: 'blob' })

export function downloadSimulationPdf(simulation) {
	const doc = new jsPDF({ unit: 'mm', format: 'a4' })
	const money = (value) => `$${Number(value).toFixed(2)}`
	const margin = 14
	let y = 18

	doc.setFont('helvetica', 'bold')
	doc.setFontSize(18)
	doc.text('Credit Simulator', margin, y)
	y += 9
	doc.setFontSize(13)
	doc.text('Detalle de amortizacion', margin, y)
	y += 10

	doc.setFont('helvetica', 'normal')
	doc.setFontSize(10)
	const details = [
		`Tipo de credito: ${simulation.tipoCredito}`,
		`Metodo: ${simulation.metodo}`,
		`Monto: ${money(simulation.monto)}`,
		`Tasa anual: ${Number(simulation.tasaAnual).toFixed(2)}%`,
		`Plazo: ${simulation.plazoMeses} meses`,
		`Total intereses: ${money(simulation.totalIntereses)}`,
		`Total pagado: ${money(simulation.totalPagado)}`,
		`Fecha: ${new Date(simulation.createdAt).toLocaleString('es-MX')}`,
	]
	details.forEach((detail, index) => doc.text(detail, margin + (index % 2) * 92, y + Math.floor(index / 2) * 7))
	y += Math.ceil(details.length / 2) * 7 + 8

	const columns = [14, 48, 88, 124, 160]
	const headers = ['Cuota', 'Pago', 'Interes', 'Capital', 'Saldo']
	const drawHeader = () => {
		doc.setFillColor(202, 77, 50)
		doc.setTextColor(255, 255, 255)
		doc.rect(margin, y - 5, 182, 8, 'F')
		headers.forEach((header, index) => doc.text(header, columns[index], y))
		doc.setTextColor(30, 39, 33)
		y += 8
	}

	drawHeader()
	doc.setFontSize(8)
	simulation.installments.forEach((installment) => {
		if (y > 280) { doc.addPage(); y = 18; drawHeader() }
		const values = [installment.numeroCuota, money(installment.cuota), money(installment.interes), money(installment.abonoCapital), money(installment.saldo)]
		values.forEach((value, index) => doc.text(String(value), columns[index], y))
		doc.setDrawColor(225, 223, 213)
		doc.line(margin, y + 2, 196, y + 2)
		y += 7
	})

	doc.save(`simulation-${simulation.id}.pdf`)
}
