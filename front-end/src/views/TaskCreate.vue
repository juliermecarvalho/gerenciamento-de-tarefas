<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import TaskForm from '../components/TaskForm.vue'
import { getTaskById } from '../services/taskService'

const route = useRoute()
const taskId = route.query.id
const task = ref(null)
const loading = ref(false)

onMounted(async () => {
  if (taskId) {
    loading.value = true
    try {
      task.value = await getTaskById(taskId)
    } catch (err) {
      console.error('Erro ao carregar tarefa:', err)
    } finally {
      loading.value = false
    }
  }
})
</script>

<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">
      {{ taskId ? 'Editar Tarefa' : 'Nova Tarefa' }}
    </h1>

    <TaskForm :initial-task="task" :is-loading="loading" />
  </div>
</template>
