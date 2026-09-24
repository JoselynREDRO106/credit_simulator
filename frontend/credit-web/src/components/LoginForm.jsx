import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function LoginForm() {
	const { login } = useAuth(); const navigate = useNavigate(); const [form, setForm] = useState({ email: '', password: '' }); const [error, setError] = useState(''); const [loading, setLoading] = useState(false)
	async function submit(event) { event.preventDefault(); setError(''); setLoading(true); try { await login(form); navigate('/') } catch (requestError) { setError(requestError.response?.data?.message || 'No se pudo iniciar sesión.') } finally { setLoading(false) } }
	return <form className="form" onSubmit={submit}><label>Correo<input type="email" required value={form.email} onChange={(event) => setForm({ ...form, email: event.target.value })} /></label><label>Contraseña<input type="password" required value={form.password} onChange={(event) => setForm({ ...form, password: event.target.value })} /></label>{error && <p className="error">{error}</p>}<button disabled={loading}>{loading ? 'Entrando...' : 'Iniciar sesión'}</button><p>¿No tienes cuenta? <Link to="/register">Regístrate</Link></p></form>
}
