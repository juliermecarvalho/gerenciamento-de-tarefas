import axios from 'axios'

const BASE_URL = import.meta.env.VITE_API_BASE_URL

const api = axios.create({
  baseURL: `${BASE_URL}/api/v1`,
  headers: {
    'Accept': 'application/json',
    'x-api-version': '1.0'
  }
})

// Interceptor para adicionar o token
api.interceptors.request.use(
  config => {
    const token = localStorage.getItem('auth_token')
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`
    }
    return config
  },
  error => Promise.reject(error)
)

// Interceptor para tratar erros globais
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response && error.response.status === 401) {
      // Token inválido ou expirado
      localStorage.removeItem('auth_token') // limpa o token
      
      // Redireciona para login
      window.location.href = '/login' // ajuste conforme sua rota
    }

    return Promise.reject(error)
  }
)

export default api
