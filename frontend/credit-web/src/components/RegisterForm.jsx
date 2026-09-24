import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function RegisterForm() {
	const { register } = useAuth(); const navigate = useNavigate(); const [form, setForm] = useState({ nombre: '', email: '', password: '' }); const [error, setError] = useState('')
	async function submit(event) { event.preventDefault(); setError(''); try { await register(form); navigate('/') } catch (requestError) { setError(requestError.response?.data?.message || 'No se pudo registrar la cuenta.') } }
	return <form className="form" onSubmit={submit}><label>Nombre<input required minLength="2" value={form.nombre} onChange={(event) => setForm({ ...form, nombre: event.target.value })} /></label><label>Correo<input type="email" required value={form.email} onChange={(event) => setForm({ ...form, email: event.target.value })} /></label><label>Contraseña<input type="password" required minLength="6" value={form.password} onChange={(event) => setForm({ ...form, password: event.target.value })} /></label>{error && <p className="error">{error}</p>}<button>Crear cuenta</button><p>¿Ya tienes cuenta? <Link to="/login">Inicia sesión</Link></p></form>
}
