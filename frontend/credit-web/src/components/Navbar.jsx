import { Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Navbar() { const { session, logout } = useAuth(); return <header className="navbar"><Link className="brand" to="/">Credit Simulator</Link><nav><Link to="/">Nueva simulación</Link><Link to="/history">Historial</Link><span>{session?.user?.nombre}</span><button className="link-button" onClick={logout}>Salir</button></nav></header> }
