<template>
  <div>
    <!-- Navegação (somente com token válido) -->
    <nav
      v-if="userName && route.path !== '/login'"
      class="bg-gray-100 p-4 flex justify-between items-center space-x-4"
    >
      <div class="flex space-x-4">
        <router-link to="/tasks" class="text-blue-600 hover:underline"
          >Tarefas</router-link
        >
        <router-link to="/users" class="text-blue-600 hover:underline"
          >Usuários</router-link
        >
      </div>
      <span class="text-gray-700 font-medium">Bem-vindo, {{ userName }}</span>

      <div class="flex items-center space-x-2">
        <span class="text-gray-700 font-medium">Bem-vindo, {{ userName }}</span>
        <button
          @click="logout"
          class="text-red-600 border border-red-500 px-2 py-1 rounded hover:bg-red-100 transition"
        >
          Sair
        </button>
      </div>
    </nav>

    <!-- Notificações (somente com token válido) -->
    <div
      v-if="userName && notifications.length > 0 && route.path !== '/login'"
      class="fixed top-4 right-4 z-50 w-80 space-y-2"
    >
      <div
        v-for="(msg, i) in notifications"
        :key="i"
        class="bg-green-100 border border-green-400 text-green-800 px-4 py-2 rounded shadow"
      >
        {{ msg }}
      </div>
    </div>

    <!-- Conteúdo das rotas -->
    <router-view />
  </div>
</template>

<script setup>
import { onMounted, onBeforeUnmount, ref } from "vue";
import { decodeToken } from "./services/authService"; // ajuste o caminho se necessário
import { useRoute } from "vue-router";

const notifications = ref([]);
const userName = ref("");
let eventSource = null;
const route = useRoute();

onMounted(() => {
  const user = decodeToken();
  if (user && user.name) {
    userName.value = user.name;

    // Abrir conexão SSE
    const BASE_URL = import.meta.env.VITE_API_BASE_URL;
    const API = `${BASE_URL}/api/v1/Notification/stream`;
    eventSource = new EventSource(API);

    eventSource.onmessage = (event) => {
      notifications.value.push(event.data);
      setTimeout(() => {
        notifications.value.shift();
      }, 5000);
    };

    eventSource.onerror = (error) => {
      console.error("SSE Error:", error);
      eventSource.close();
    };
  }
});

onBeforeUnmount(() => {
  eventSource?.close();
});

// Função para logout
function logout() {
  localStorage.removeItem('auth_token');
  window.location = '/login'; // Redireciona para a página de login
   // Remove o token do armazenamento local
}
</script>
