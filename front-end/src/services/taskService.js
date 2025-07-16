import axios from 'axios'

const API = 'http://localhost:5000/api/v1/Task'

export function createTask(task) {
  return axios.post(API, task, {
    headers: {
      'Content-Type': 'application/json',
      'x-api-version': '1.0'
    }
  })
}

export function getTasks() {
  return axios.get(API, {
    headers: {
      'Accept': 'text/plain',
      'x-api-version': '1.0'
    }
  }).then(res => res.data)
}

export function deleteTask(id) {
  return axios.delete(`${API}/${id}`, {
    headers: { 'x-api-version': '1.0' }
  })
}

export function updateTask(id, data) {
  return axios.put(`${API}/${id}`, data, {
    headers: {
      'Content-Type': 'application/json',
      'x-api-version': '1.0'
    }
  })
}

export function getTaskById(id) {
  return axios.get(`${API}/${id}`, {
    headers: {
      'Accept': 'text/plain',
      'x-api-version': '1.0'
    }
  }).then(res => res.data)
}
