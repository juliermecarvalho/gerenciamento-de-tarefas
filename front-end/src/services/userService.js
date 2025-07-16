import axios from 'axios'

const BASE_URL = import.meta.env.VITE_API_BASE_URL
const API = `${BASE_URL}/api/v1/User`

export function getUsers() {
  return axios.get(API, {
    headers: {
      'Accept': 'text/plain',
      'x-api-version': '1.0'
    }
  }).then(res => res.data)
}

export function createUser(user) {
  return axios.post(`${API}`, user, {
    headers: {
      'Content-Type': 'application/json',
      'x-api-version': '1.0'
    }
  })
}

export function getUserById(id) {
  return axios.get(`${API}/${id}`, {
    headers: {
      'Accept': 'text/plain',
      'x-api-version': '1.0'
    }
  }).then(res => res.data)
}

export function deleteUser(id) {
  return axios.delete(`${API}/${id}`, {
    headers: { 'x-api-version': '1.0' }
  })
}
