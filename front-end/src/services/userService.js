import api from './axiosInstance'

export function getUsers() {
  return api.get('/User').then(res => res.data)
}

export function createUser(user) {
  return api.post('/User', user)
}

export function getUserById(id) {
  return api.get(`/User/${id}`).then(res => res.data)
}

export function deleteUser(id) {
  return api.delete(`/User/${id}`)
}

export function createRandomUsers(amount = 10, userNameMask = "user_{{random}}") {
  return api.post('/User/createRandom', {
    amount,
    userNameMask
  })
}
