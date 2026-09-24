import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import Navbar from '../components/Navbar'
import { getSimulations } from '../services/simulationService'

export default function HistoryPage() { const [items, setItems] = useState([]); const [error, setError] = useState(''); useEffect(() => { getSimulations().then(({ data }) => setItems(data)).catch(() => setError('No se pudo cargar el historial.')) }, []); return <><Navbar /><main className="page"><div className="page-heading"><p className="eyebrow">HISTORIAL</p><h1>Tus simulaciones.</h1></div>{error && <p className="error">{error}</p>}<section className="history-list">{items.map((item) => <Link className="history-item" key={item.id} to={`/simulations/${item.id}`}><span>{new Date(item.createdAt).toLocaleDateString('es-MX')}</span><strong>{item.tipoCredito} · {item.metodo}</strong><span>${item.totalPagado.toFixed(2)}</span></Link>)}{!items.length && !error && <div className="empty"><h2>Aún no tienes simulaciones.</h2><Link to="/">Crear una simulación</Link></div>}</section></main></> }
