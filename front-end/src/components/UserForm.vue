<script setup>
import { ref, watchEffect } from 'vue'
import { useRouter } from 'vue-router'
import { useForm, useField } from 'vee-validate'
import { z } from 'zod'
import { toTypedSchema } from '@vee-validate/zod'
import { createUser } from '../services/userService'

const props = defineProps({
  initialUser: {
    type: Object,
    default: null
  },
  isLoading: Boolean
})

const router = useRouter()

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório'),
  email: z.string().email('Email inválido')
})

const { handleSubmit } = useForm({
  validationSchema: toTypedSchema(schema)
})

const { value: name, errorMessage: nameError } = useField('name')
const { value: email, errorMessage: emailError } = useField('email')

const isSaving = ref(false)
const message = ref('')
const messageType = ref('')

watchEffect(() => {
  if (props.initialUser) {
    name.value = props.initialUser.name || ''
    email.value = props.initialUser.email || ''
  }
})

const onSubmit = handleSubmit(async (values) => {
  isSaving.value = true
  message.value = ''
  messageType.value = ''

  try {
    // Para edição futura, pode adicionar verificação de ID
    await createUser(values)
    message.value = 'Usuário criado com sucesso!'
    messageType.value = 'success'
    setTimeout(() => router.push('/users'), 1000)
  } catch (error) {
    message.value = 'Erro ao salvar usuário'
    messageType.value = 'error'
  } finally {
    isSaving.value = false
  }
})
</script>

<template>
  <form @submit.prevent="onSubmit" class="space-y-4 max-w-md mx-auto p-6 bg-white shadow rounded">
    <div>
      <label class="block font-medium">Nome</label>
      <input v-model="name" class="w-full p-2 border rounded" />
      <span class="text-red-500 text-sm">{{ nameError }}</span>
    </div>

    <div>
      <label class="block font-medium">Email</label>
      <input v-model="email" class="w-full p-2 border rounded" />
      <span class="text-red-500 text-sm">{{ emailError }}</span>
    </div>

    <button
      type="submit"
      :disabled="isSaving || isLoading"
      class="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 transition disabled:opacity-50"
    >
      {{ isSaving ? 'Salvando...' : 'Salvar' }}
    </button>

    <p v-if="message" :class="{
      'text-green-600 mt-2': messageType === 'success',
      'text-red-600 mt-2': messageType === 'error'
    }">
      {{ message }}
    </p>
  </form>
</template>
