import axios from 'axios'

const catalogApi = axios.create({
	baseURL: import.meta.env.VITE_CATALOG_API_URL || 'http://localhost:5002/api',
})

export const getCreditTypes = () => catalogApi.get('/credit-types')
