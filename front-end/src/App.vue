<template>
  <nav class="bg-gray-100 p-4 flex space-x-4">
    <router-link to="/tasks" class="text-blue-600 hover:underline">Tarefas</router-link>
    <router-link to="/task/create" class="text-blue-600 hover:underline">Nova Tarefa</router-link>
    <router-link to="/users" class="text-blue-600 hover:underline">Usuários</router-link>
  </nav>
  <router-view />
</template>
