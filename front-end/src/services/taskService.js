import api from './axiosInstance'

export function createTask(task) {
  return api.post('/Task', task)
}

export function getTasks() {
  return api.get('/Task').then(res => res.data)
}

export function deleteTask(id) {
  return api.delete(`/Task/${id}`)
}

export function updateTask(id, data) {
  return api.put(`/Task/${id}`, data)
}

export function getTaskById(id) {
  return api.get(`/Task/${id}`).then(res => res.data)
}

export function assignUserToTask(taskId, userId) {
  return api.post('/Task/assign', { taskId, userId })
}
