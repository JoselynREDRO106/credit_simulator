import { createContext, useContext, useMemo, useState } from 'react'
import * as authService from '../services/authService'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
	const [session, setSession] = useState(() => {
		const token = localStorage.getItem('credit_token')
		const user = JSON.parse(localStorage.getItem('credit_user') || 'null')
		return token && user ? { token, user } : null
	})

	const saveSession = (data) => {
		localStorage.setItem('credit_token', data.token)
		localStorage.setItem('credit_user', JSON.stringify(data))
		setSession({ token: data.token, user: data })
	}

	const value = useMemo(() => ({
		session,
		isAuthenticated: Boolean(session),
		async login(credentials) {
			const { data } = await authService.login(credentials)
			saveSession(data)
		},
		async register(details) {
			const { data } = await authService.register(details)
			saveSession(data)
		},
		logout() {
			localStorage.removeItem('credit_token')
			localStorage.removeItem('credit_user')
			setSession(null)
		},
	}), [session])

	return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export const useAuth = () => useContext(AuthContext)
