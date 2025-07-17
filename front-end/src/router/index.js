import { createRouter, createWebHistory } from 'vue-router'
import TaskCreate from '../views/TaskCreate.vue'
import TaskList from '../views/TaskList.vue'
import UserList from '../views/UserList.vue'
import UserCreate from '../views/UserCreate.vue'
import Login from '../views/Login.vue'
import { getToken, decodeToken } from '../services/authService'

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', name: 'Login', component: Login },
  { path: '/tasks', name: 'TaskList', component: TaskList, meta: { requiresAuth: true } },
  { path: '/task/create', name: 'TaskCreate', component: TaskCreate , meta: { requiresAuth: true }},
  { path: '/users', name: 'UserList', component: UserList, meta: { requiresAuth: true } },
  { path: '/users/create', name: 'UserCreate', component: UserCreate, meta: { requiresAuth: true } }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  const token = getToken()
  const isValid = !!decodeToken() // se conseguir decodificar, assume que está OK

  if (requiresAuth && (!token || !isValid)) {
    next('/login')
  } else {
    next()
  }
})

export default router
