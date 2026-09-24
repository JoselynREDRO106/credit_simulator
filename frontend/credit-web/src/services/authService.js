import axios from 'axios'

const authApi = axios.create({
	baseURL: import.meta.env.VITE_AUTH_API_URL || 'http://localhost:5001/api',
})

export const register = (data) => authApi.post('/auth/register', data)
export const login = (data) => authApi.post('/auth/login', data)
