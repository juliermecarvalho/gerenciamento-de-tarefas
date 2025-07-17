import axios from 'axios'
import { jwtDecode } from 'jwt-decode'

const BASE_URL = import.meta.env.VITE_API_BASE_URL
const API = `${BASE_URL}/api/v1/Login` // ⬅️ Ajuste aqui, padrão igual ao /Task

// Faz login e armazena o token JWT no localStorage
export function login(email, password) {
  return axios.post(API, {
    email,
    password
  }, {
    headers: {
      'Content-Type': 'application/json',
      'x-api-version': '1.0'
    }
  })
    .then(res => {
        debugger;
      const token = res.data.token
      localStorage.setItem('auth_token', token)
      return token
    })
    .catch(err => {
      console.error('Erro ao fazer login:', err)
      throw new Error('Credenciais inválidas ou erro de conexão')
    })
}

// Recupera o token salvo no localStorage
export function getToken() {
  return localStorage.getItem('auth_token')
}

// Decodifica o token JWT e retorna os dados
export function decodeToken() {
  const token = getToken()
  if (!token) return null

  try {
    return jwtDecode(token)
  } catch {
    return null
  }
}

// Remove o token do localStorage (logout)
export function logout() {
  localStorage.removeItem('auth_token')
}
