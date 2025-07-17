<script setup>
import { ref } from "vue"
import { useRouter } from "vue-router"
import { login as loginRequest, decodeToken } from "../services/authService" // ✅ Importa funções individuais

const email = ref("")
const password = ref("")
const error = ref("")
const router = useRouter()

async function login() {
  error.value = ""
  try {
    const token = await loginRequest(email.value, password.value)
    const user = decodeToken(token)
    console.log("Usuário autenticado:", user)
    router.push("/users")
  } catch (err) {
    error.value = err.message
  }
}
</script>

<template>
  <div class="max-w-md mx-auto mt-20 p-6 bg-white shadow rounded">
    <h1 class="text-2xl font-bold mb-4">Login</h1>

    <div v-if="error" class="text-red-600 mb-4">{{ error }}</div>

    <form @submit.prevent="login" class="space-y-4">
      <div>
        <label for="email" class="block font-semibold">Email</label>
        <input
          v-model="email"
          type="email"
          id="email"
          class="w-full border px-3 py-2 rounded"
          required
        />
      </div>

      <div>
        <label for="password" class="block font-semibold">Senha</label>
        <input
          v-model="password"
          type="password"
          id="password"
          class="w-full border px-3 py-2 rounded"
          required
        />
      </div>

      <button
        type="submit"
        class="w-full bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Entrar
      </button>
    </form>
  </div>
</template>
