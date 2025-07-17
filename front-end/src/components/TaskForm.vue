<script setup>
import { ref, onMounted, watchEffect } from "vue";
import { useRouter } from "vue-router";
import { useForm, useField } from "vee-validate";
import { z } from "zod";
import { toTypedSchema } from "@vee-validate/zod";
import { createTask, updateTask } from "../services/taskService";
import { getUsers } from "../services/userService";

const props = defineProps({
  initialTask: {
    type: Object,
    default: null,
  },
  isLoading: Boolean,
});

const router = useRouter();

const schema = z.object({
  title: z.string().min(1, "Título obrigatório"),
  description: z.string().min(1, "Descrição obrigatória"),
  isCompleted: z.boolean().optional().default(false),
});

const { handleSubmit } = useForm({
  validationSchema: toTypedSchema(schema),
});

const { value: title, errorMessage: titleError } = useField("title");
const { value: description, errorMessage: descriptionError } =
  useField("description");
const { value: isCompleted } = useField("isCompleted");

const isSaving = ref(false);
const message = ref("");
const messageType = ref("");

const users = ref([]);
const responsibleName = ref("");

onMounted(async () => {
  if (props.initialTask?.userId) {
    try {
      users.value = await getUsers();
      const responsible = users.value.find(
        (u) => u.id === props.initialTask.userId
      );
      responsibleName.value = responsible?.name ?? "Nenhum responsável";
    } catch (err) {
      console.error("Erro ao carregar usuários:", err);
    }
  }
});

watchEffect(() => {
  if (props.initialTask) {
    title.value = props.initialTask.title || "";
    description.value = props.initialTask.description || "";
    isCompleted.value = props.initialTask.isCompleted || false;
  }
});

const onSubmit = handleSubmit(async (values) => {
  isSaving.value = true;
  message.value = "";
  messageType.value = "";

  try {
    const payload = { ...values };

    if (props.initialTask?.id) {
      await updateTask(props.initialTask.id, payload);
      message.value = "Tarefa atualizada com sucesso!";
    } else {
      await createTask(payload);
      message.value = "Tarefa criada com sucesso!";
    }

    messageType.value = "success";
    setTimeout(() => router.push("/tasks"), 1000);
  } catch (error) {
    message.value = "Erro ao salvar tarefa";
    messageType.value = "error";
  } finally {
    isSaving.value = false;
  }
});
</script>

<template>
  <form
    @submit.prevent="onSubmit"
    class="space-y-4 max-w-md mx-auto p-6 bg-white shadow rounded"
  >
    <div>
      <label class="block font-medium">Título</label>
      <input v-model="title" class="w-full p-2 border rounded" />
      <span class="text-red-500 text-sm">{{ titleError }}</span>
    </div>

    <div>
      <label class="block font-medium">Descrição</label>
      <textarea
        v-model="description"
        class="w-full p-2 border rounded"
      ></textarea>
      <span class="text-red-500 text-sm">{{ descriptionError }}</span>
    </div>

    <!-- Apenas para edição -->
    <div v-if="props.initialTask">
      <label class="block font-medium">Responsável</label>
      <p class="p-2 border rounded bg-gray-50 text-gray-700">
        {{ responsibleName || "Nenhum responsável" }}
      </p>
    </div>

    <div v-if="props.initialTask">
      <label class="block font-medium">Status</label>
      <p class="p-2 border rounded bg-gray-50 text-gray-700">
        {{ isCompleted ? "Concluída" : "Pendente" }}
      </p>
    </div>

    <button
      type="submit"
      :disabled="isSaving || isLoading"
      class="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 transition disabled:opacity-50 disabled:cursor-not-allowed"
    >
      {{ isSaving ? "Salvando..." : "Salvar" }}
    </button>

    <p
      v-if="message"
      :class="{
        'text-green-600 mt-2': messageType === 'success',
        'text-red-600 mt-2': messageType === 'error',
      }"
    >
      {{ message }}
    </p>
  </form>
</template>
