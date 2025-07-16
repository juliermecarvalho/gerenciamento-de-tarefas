<script setup>
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { getTasks, deleteTask, updateTask } from '../services/taskService'

const router = useRouter()
const tasks = ref([])
const loading = ref(true)
const error = ref(null)

async function loadTasks() {
  loading.value = true
  error.value = null
  try {
    tasks.value = await getTasks()
  } catch (err) {
    error.value = 'Erro ao buscar tarefas'
  } finally {
    loading.value = false
  }
}

async function handleDelete(id) {
  if (confirm('Deseja excluir esta tarefa?')) {
    await deleteTask(id)
    await loadTasks()
  }
}

async function handleComplete(task) {
  await updateTask(task.id, {
    title: task.title,
    description: task.description,
    isCompleted: true
  })
  await loadTasks()
}

function handleEdit(task) {
  router.push({ name: 'TaskCreate', query: { id: task.id } })
}

onMounted(loadTasks)
</script>

<template>
  <div class="max-w-5xl mx-auto mt-8 p-6 bg-white rounded shadow">
    <h1 class="text-2xl font-bold mb-4">Lista de Tarefas</h1>

    <p v-if="loading">Carregando...</p>
    <p v-if="error" class="text-red-500">{{ error }}</p>

    <table v-if="!loading && tasks.length" class="w-full table-auto border">
      <thead>
        <tr class="bg-gray-100 text-left">
          <th class="p-2 border">Título</th>
          <th class="p-2 border">Descrição</th>
          <th class="p-2 border">Responsável</th>
          <th class="p-2 border">Status</th>
          <th class="p-2 border text-center">Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="task in tasks" :key="task.id" class="border-b">
          <td class="p-2 border">{{ task.title }}</td>
          <td class="p-2 border">{{ task.description }}</td>
          <td class="p-2 border">{{ task.name ?? '---' }}</td>
          <td class="p-2 border">
            <span :class="task.isCompleted ? 'text-green-600' : 'text-yellow-500'">
              {{ task.isCompleted ? 'Concluída' : 'Pendente' }}
            </span>
          </td>
          <td class="p-2 border flex gap-2 justify-center">
            <button
              @click="handleComplete(task)"
              :disabled="task.isCompleted"
              class="text-sm px-2 py-1 rounded bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50"
            >
              Finalizar
            </button>
            <button
              @click="handleDelete(task.id)"
              :disabled="task.isCompleted"
              class="text-sm px-2 py-1 rounded bg-red-600 text-white hover:bg-red-700 disabled:opacity-50"
            >
              Deletar
            </button>
            <button
              :disabled="task.isCompleted"
              class="text-sm px-2 py-1 rounded bg-gray-200 hover:bg-gray-300 disabled:opacity-50"
              title="Atribuir usuário"
            >
              👤
            </button>
            <button
              @click="handleEdit(task)"
              :disabled="task.isCompleted"
              class="text-sm px-2 py-1 rounded bg-gray-200 hover:bg-gray-300 disabled:opacity-50"
              title="Editar tarefa"
            >
              ✏️
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else-if="!loading">Nenhuma tarefa encontrada.</p>
  </div>
</template>
