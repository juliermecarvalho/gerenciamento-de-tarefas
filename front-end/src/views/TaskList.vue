<script setup>
import { onMounted, ref, computed } from "vue";
import { useRouter } from "vue-router";
import {
  getTasksPaged,
  deleteTask,
  updateTask,
  assignUserToTask,
} from "../services/taskService";
import { getUsers } from "../services/userService";
import { createRandomTasks } from "../services/taskService";

const generatingTasks = ref(false);
const router = useRouter();
const tasks = ref([]);
const users = ref([]);
const loading = ref(true);
const error = ref(null);

const currentPage = ref(1);
const totalRecords = ref(0);
const pageSize = 10;

const showModal = ref(false);
const selectedTask = ref(null);
const selectedUserId = ref("");

const totalPages = computed(() => Math.ceil(totalRecords.value / pageSize));

async function loadTasks() {
  loading.value = true;
  error.value = null;
  try {
    const data = await getTasksPaged(currentPage.value);
    tasks.value = data.items;
    totalRecords.value = data.totalRecords;
  } catch (err) {
    error.value = "Erro ao buscar tarefas";
  } finally {
    loading.value = false;
  }
}

async function loadUsers() {
  try {
    users.value = await getUsers();
  } catch (err) {
    console.error("Erro ao carregar usuários", err);
  }
}

function openAssignModal(task) {
  selectedTask.value = task;
  selectedUserId.value = task.userId || "";
  showModal.value = true;
  loadUsers();
}

async function confirmAssign() {
  if (!selectedTask.value) return;

  try {
    await assignUserToTask(selectedTask.value.id, selectedUserId.value);
    closeModal();
    await loadTasks();
  } catch (err) {
    console.error("Erro ao atribuir usuário:", err);
    alert("Erro ao atribuir usuário à tarefa.");
  }
}

function closeModal() {
  showModal.value = false;
  selectedTask.value = null;
  selectedUserId.value = "";
}

async function handleDelete(id) {
  if (confirm("Deseja excluir esta tarefa?")) {
    await deleteTask(id);
    await loadTasks();
  }
}

async function handleComplete(task) {
  await updateTask(task.id, {
    title: task.title,
    description: task.description,
    isCompleted: true,
  });
  await loadTasks();
}

function handleEdit(task) {
  router.push({ name: "TaskCreate", query: { id: task.id } });
}

function nextPage() {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
    loadTasks();
  }
}

function prevPage() {
  if (currentPage.value > 1) {
    currentPage.value--;
    loadTasks();
  }
}

async function handleGenerateTasks() {
  generatingTasks.value = true;
  try {
    await createRandomTasks();
    currentPage.value = 1;
    await loadTasks();
  } catch (err) {
    console.error("Erro ao gerar tarefas:", err);
    alert("Erro ao gerar tarefas aleatórias.");
  } finally {
    generatingTasks.value = false;
  }
}

onMounted(loadTasks);
</script>

<template>
  <div class="max-w-5xl mx-auto mt-8 p-6 bg-white rounded shadow">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">Lista de Tarefas</h1>
      <div class="flex space-x-2">
        <button
          @click="router.push('/task/create')"
          class="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
        >
          Nova Tarefa
        </button>
        <button
          @click="handleGenerateTasks"
          :disabled="generatingTasks"
          class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 disabled:opacity-50"
        >
          {{ generatingTasks ? "Gerando..." : "Gerar 1000 Tarefas" }}
        </button>
      </div>
    </div>

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
          <td class="p-2 border">{{ task.name ?? "---" }}</td>
          <td class="p-2 border">
            <span
              :class="task.isCompleted ? 'text-green-600' : 'text-yellow-500'"
            >
              {{ task.isCompleted ? "Concluída" : "Pendente" }}
            </span>
          </td>
          <td class="p-2 border text-center">
            <div class="flex justify-center space-x-2">
              <button
                @click="handleComplete(task)"
                :disabled="task.isCompleted"
                class="text-sm px-2 py-1 rounded bg-gray-100 hover:bg-gray-300 disabled:opacity-50"
                title="Finalizar tarefa"
              >
                ✅
              </button>

              <button
                @click="handleDelete(task.id)"
                :disabled="task.isCompleted"
                class="text-sm px-2 py-1 rounded bg-gray-100 hover:bg-gray-300 disabled:opacity-50"
                title="Excluir tarefa"
              >
                🗑️
              </button>

              <button
                @click="openAssignModal(task)"
                :disabled="task.isCompleted"
                class="text-sm px-2 py-1 rounded bg-gray-100 hover:bg-gray-300 disabled:opacity-50"
                title="Atribuir usuário"
              >
                👤
              </button>

              <button
                @click="handleEdit(task)"
                :disabled="task.isCompleted"
                class="text-sm px-2 py-1 rounded bg-gray-100 hover:bg-gray-300 disabled:opacity-50"
                title="Editar tarefa"
              >
                ✏️
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else-if="!loading">Nenhuma tarefa encontrada.</p>

    <!-- Paginação -->
    <div v-if="totalPages > 1" class="mt-4 flex justify-center space-x-2">
      <button
        @click="prevPage"
        :disabled="currentPage === 1"
        class="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
      >
        ◀ Anterior
      </button>

      <span class="px-4 py-2 font-semibold">
        Página {{ currentPage }} de {{ totalPages }}
      </span>

      <button
        @click="nextPage"
        :disabled="currentPage === totalPages"
        class="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
      >
        Próxima ▶
      </button>
    </div>

    <!-- Modal de atribuição -->
    <div
      v-if="showModal"
      class="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50"
    >
      <div class="bg-white p-6 rounded shadow w-full max-w-sm">
        <h2 class="text-lg font-bold mb-4">Atribuir Usuário</h2>

        <label class="block mb-2">Selecione um usuário:</label>
        <select v-model="selectedUserId" class="w-full p-2 border rounded mb-4">
          <option value="">Nenhum responsável</option>
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.name }}
          </option>
        </select>

        <div class="flex justify-end space-x-2">
          <button
            @click="closeModal"
            class="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400"
          >
            Cancelar
          </button>
          <button
            @click="confirmAssign"
            class="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
          >
            Confirmar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
