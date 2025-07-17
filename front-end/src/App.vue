<template>
  <div>
    <!-- Navegação -->
    <nav class="bg-gray-100 p-4 flex space-x-4">
      <router-link to="/tasks" class="text-blue-600 hover:underline"
        >Tarefas</router-link
      >
   
      <router-link to="/users" class="text-blue-600 hover:underline"
        >Usuários</router-link
      >
    </nav>

    <!-- Notificações -->
    <div
      v-if="notifications.length > 0"
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

<script setup lang="ts">
import { onMounted, onBeforeUnmount, ref } from "vue";

const notifications = ref<string[]>([]);
let eventSource: EventSource;

onMounted(() => {
  const BASE_URL = import.meta.env.VITE_API_BASE_URL;
  const API = `${BASE_URL}/api/v1/Notification/stream`;
  eventSource = new EventSource(API);
  debugger;
  eventSource.onmessage = (event) => {
    notifications.value.push(event.data);
    // Remover notificação automaticamente após 5s
    setTimeout(() => {
      notifications.value.shift();
    }, 5000);
  };

  eventSource.onerror = (error) => {
    console.error("SSE Error:", error);
    eventSource.close();
  };
});

onBeforeUnmount(() => {
  eventSource?.close();
});
</script>
