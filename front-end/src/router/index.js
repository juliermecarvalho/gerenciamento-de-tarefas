import { createRouter, createWebHistory } from 'vue-router'
import TaskCreate from '../views/TaskCreate.vue'
import TaskList from '../views/TaskList.vue'

const routes = [
  { path: '/', redirect: '/tasks' },
  { path: '/tasks', name: 'TaskList', component: TaskList },
  { path: '/create', name: 'TaskCreate', component: TaskCreate }
]

export default createRouter({
  history: createWebHistory(),
  routes
})
