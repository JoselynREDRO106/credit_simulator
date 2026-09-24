import { Navigate, Route, Routes } from 'react-router-dom'
import PrivateRoute from './components/PrivateRoute'
import { useAuth } from './context/AuthContext'
import HistoryPage from './pages/HistoryPage'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import SimulationDetailPage from './pages/SimulationDetailPage'
import SimulatorPage from './pages/SimulatorPage'

export default function App() {
	const { isAuthenticated } = useAuth()
	return (
		<Routes>
			<Route path="/login" element={isAuthenticated ? <Navigate to="/" replace /> : <LoginPage />} />
			<Route path="/register" element={isAuthenticated ? <Navigate to="/" replace /> : <RegisterPage />} />
			<Route element={<PrivateRoute />}>
				<Route path="/" element={<SimulatorPage />} />
				<Route path="/history" element={<HistoryPage />} />
				<Route path="/simulations/:id" element={<SimulationDetailPage />} />
			</Route>
			<Route path="*" element={<Navigate to={isAuthenticated ? '/' : '/login'} replace />} />
		</Routes>
	)
}
