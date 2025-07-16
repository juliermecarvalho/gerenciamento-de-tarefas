<script setup>
import { ref, onMounted, watchEffect } from 'vue'
import { useRouter } from 'vue-router'
import { useForm, useField } from 'vee-validate'
import { z } from 'zod'
import { toTypedSchema } from '@vee-validate/zod'
import { createTask, updateTask } from '../services/taskService'

const props = defineProps({
  initialTask: {
    type: Object,
    default: null
  },
  isLoading: Boolean
})

const router = useRouter()

const schema = z.object({
  title: z.string().min(1, 'Título obrigatório'),
  description: z.string().min(1, 'Descrição obrigatória'),
  userId: z.string().uuid('ID inválido').optional().or(z.literal('')),
  isCompleted: z.boolean().optional().default(false),
})

const { handleSubmit } = useForm({
  validationSchema: toTypedSchema(schema),
})

const { value: title, errorMessage: titleError } = useField('title')
const { value: description, errorMessage: descriptionError } = useField('description')
const { value: userId, errorMessage: userIdError } = useField('userId')
const { value: isCompleted } = useField('isCompleted')

const isSaving = ref(false)
const message = ref('')
const messageType = ref('')

watchEffect(() => {
  if (props.initialTask) {
    title.value = props.initialTask.title || ''
    description.value = props.initialTask.description || ''
    userId.value = props.initialTask.userId || ''
    isCompleted.value = props.initialTask.isCompleted || false
  }
})

const onSubmit = handleSubmit(async (values) => {
  isSaving.value = true
  message.value = ''
  messageType.value = ''

  try {
    const payload = { ...values }
    if (!payload.userId) delete payload.userId

    if (props.initialTask?.id) {
      await updateTask(props.initialTask.id, payload)
      message.value = 'Tarefa atualizada com sucesso!'
    } else {
      await createTask(payload)
      message.value = 'Tarefa criada com sucesso!'
    }

    messageType.value = 'success'
    setTimeout(() => router.push('/tasks'), 1000)
  } catch (error) {
    message.value = 'Erro ao salvar tarefa'
    messageType.value = 'error'
  } finally {
    isSaving.value = false
  }
})
</script>

<template>
  <form @submit.prevent="onSubmit" class="space-y-4 max-w-md mx-auto p-6 bg-white shadow rounded">
    <div>
      <label class="block font-medium">Título</label>
      <input v-model="title" class="w-full p-2 border rounded" />
      <span class="text-red-500 text-sm">{{ titleError }}</span>
    </div>

    <div>
      <label class="block font-medium">Descrição</label>
      <textarea v-model="description" class="w-full p-2 border rounded"></textarea>
      <span class="text-red-500 text-sm">{{ descriptionError }}</span>
    </div>

    <div>
      <label class="block font-medium">ID do Usuário</label>
      <input v-model="userId" class="w-full p-2 border rounded" />
      <span class="text-red-500 text-sm">{{ userIdError }}</span>
    </div>

    <div>
      <label class="inline-flex items-center">
        <input type="checkbox" v-model="isCompleted" class="mr-2" />
        Concluído
      </label>
    </div>

    <button
      type="submit"
      :disabled="isSaving || isLoading"
      class="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 transition disabled:opacity-50 disabled:cursor-not-allowed"
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
