<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import UserForm from '../components/UserForm.vue'
import { getUserById } from '../services/userService'

const route = useRoute()
const userId = route.query.id
const user = ref(null)
const loading = ref(false)

onMounted(async () => {
  if (userId) {
    loading.value = true
    try {
      user.value = await getUserById(userId)
    } catch (err) {
      console.error('Erro ao carregar usuário:', err)
    } finally {
      loading.value = false
    }
  }
})
</script>

<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">
      {{ userId ? 'Editar Usuário' : 'Novo Usuário' }}
    </h1>

    <UserForm :initial-user="user" :is-loading="loading" />
  </div>
</template>
