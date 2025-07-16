import { createRouter, createWebHistory } from 'vue-router'
import TaskCreate from '../views/TaskCreate.vue'
import TaskList from '../views/TaskList.vue'
import UserList from '../views/UserList.vue'
import UserCreate from '../views/UserCreate.vue'

const routes = [
  { path: '/', redirect: '/tasks' },
  { path: '/tasks', name: 'TaskList', component: TaskList },
  { path: '/task/create', name: 'TaskCreate', component: TaskCreate },
  { path: '/users', name: 'UserList', component: UserList },
  { path: '/users/create', name: 'UserCreate', component: UserCreate }
]

export default createRouter({
  history: createWebHistory(),
  routes
})
