import axios from 'axios'

const apiClient = axios.create()

apiClient.interceptors.request.use((config) => {
	const token = localStorage.getItem('credit_token')
	if (token) config.headers.Authorization = `Bearer ${token}`
	return config
})

export default apiClient
