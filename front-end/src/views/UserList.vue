<script setup>
import { ref, computed, onMounted } from "vue";
import { getUsersPaged, deleteUser, createRandomUsers } from "../services/userService";
import { useRouter } from "vue-router";

const users = ref([]);
const loading = ref(true);
const error = ref(null);
const generatingUsers = ref(false);
const currentPage = ref(1);
const totalRecords = ref(0);
const pageSize = 10;

const router = useRouter();

async function loadUsers() {
  loading.value = true;
  error.value = null;
  try {
    const data = await getUsersPaged(currentPage.value);
    users.value = data.items;
    totalRecords.value = data.totalRecords;
  } catch (err) {
    error.value = "Erro ao buscar usuários";
  } finally {
    loading.value = false;
  }
}

async function handleDelete(id) {
  if (confirm("Deseja realmente excluir este usuário?")) {
    await deleteUser(id);
    await loadUsers();
  }
}

function handleEdit(user) {
  router.push({ path: "/users/create", query: { id: user.id } });
}

async function handleGenerateUsers() {
  generatingUsers.value = true;
  try {
    await createRandomUsers(1000);
    currentPage.value = 1;
    await loadUsers();
  } catch {
    error.value = "Erro ao gerar usuários aleatórios";
  } finally {
    generatingUsers.value = false;
  }
}

function nextPage() {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
    loadUsers();
  }
}

function prevPage() {
  if (currentPage.value > 1) {
    currentPage.value--;
    loadUsers();
  }
}

const totalPages = computed(() => Math.ceil(totalRecords.value / pageSize));

onMounted(loadUsers);
</script>

<template>
  <div class="max-w-4xl mx-auto mt-8 p-6 bg-white rounded shadow">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">Lista de Usuários</h1>
      <div class="flex space-x-2">
        <button
          @click="router.push('/users/create')"
          class="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
        >
          Novo Usuário
        </button>
        <button
          @click="handleGenerateUsers"
          :disabled="generatingUsers"
          class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 disabled:opacity-50 flex items-center"
        >
          <span>{{ generatingUsers ? "Gerando..." : "Gerar 1000 Usuários" }}</span>
        </button>
      </div>
    </div>

    <p v-if="loading">Carregando...</p>
    <p v-if="error" class="text-red-500">{{ error }}</p>

    <table v-if="!loading && users.length" class="w-full table-auto border">
      <thead>
        <tr class="bg-gray-100 text-left">
          <th class="p-2 border">Nome</th>
          <th class="p-2 border">Email</th>
          <th class="p-2 border text-center">Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id" class="border-b">
          <td class="p-2 border">{{ user.name }}</td>
          <td class="p-2 border">{{ user.email }}</td>
          <td class="p-2 border text-center">
            <div class="flex justify-center space-x-2">
              <button
                @click="handleEdit(user)"
                class="text-sm px-2 py-1 rounded bg-gray-100 hover:bg-gray-300"
                title="Editar usuário"
              >
                ✏️
              </button>
              <button
                @click="handleDelete(user.id)"
                class="text-sm px-2 py-1 rounded bg-gray-100 hover:bg-gray-300"
                title="Excluir usuário"
              >
                🗑️
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else-if="!loading">Nenhum usuário encontrado.</p>

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
  </div>
</template>
