import axios from 'axios'


const BASE_URL = import.meta.env.VITE_API_BASE_URL 
const API = `${BASE_URL}/api/v1/Task`

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

export async function assignUserToTask(taskId, userId) {
  const response = await fetch(`${API}/assign`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'x-api-version': '1.0'
    },
    body: JSON.stringify({ taskId, userId })
  });
debugger;
  if (!response.ok) {
    throw new Error('Erro ao atribuir usuário à tarefa');
  }

  return await response.ok;
}
