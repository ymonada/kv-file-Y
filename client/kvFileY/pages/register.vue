<script setup lang="ts">
import type { RegisterDto } from "~/types/registerdto";
import { registerFetch } from "~/composables/service/authService";

interface RegisterForm {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}
const router = useRouter();
const data = ref<RegisterForm>({
  userName: "",
  email: "",
  password: "",
  confirmPassword: ""
});
const errorMessage = ref<string>("");
const isLoading = ref<boolean>(false);

const handleSubmit = async (e: Event) => {
  e.preventDefault();
  errorMessage.value = "";

  // Валідація
  if (!data.value.email || !data.value.password || !data.value.userName) {
    errorMessage.value = "Будь ласка, заповніть всі обов'язкові поля";
    return;
  }

  if (!/^\S+@\S+\.\S+$/.test(data.value.email)) {
    errorMessage.value = "Будь ласка, введіть коректний email";
    return;
  }

  if (data.value.password !== data.value.confirmPassword) {
    errorMessage.value = "Паролі не співпадають";
    return;
  }

  if (data.value.password.length < 6) {
    errorMessage.value = "Пароль повинен містити щонайменше 6 символів";
    return;
  }

  try {
    isLoading.value = true;
    const { error } = await registerFetch({
      email: data.value.email,
      password: data.value.password,
      userName: data.value.userName
    });

    if (error) {
      errorMessage.value = error;
      return;
    }

    await router.push("/login");
  } catch (err) {
    errorMessage.value = "Сталася помилка при реєстрації";
    console.error("Registration error:", err);
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8 bg-white p-8 rounded-lg shadow-md">
      <div class="text-center">
        <h1 class="text-3xl font-extrabold text-gray-900">Реєстрація</h1>
        <p class="mt-2 text-sm text-gray-600">
          Вже маєте акаунт?
          <NuxtLink to="/login" class="font-medium text-indigo-600 hover:text-indigo-500">
            Увійти
          </NuxtLink>
        </p>
      </div>

      <form class="mt-8 space-y-6" @submit="handleSubmit">
        <div class="rounded-md shadow-sm space-y-4">
          <div>
            <label for="username" class="block text-sm font-medium text-gray-700 mb-1">Ім'я користувача</label>
            <input id="username" v-model="data.userName" type="text" autocomplete="username" required
              class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Ваше ім'я" />
          </div>
          <div>
            <label for="email" class="block text-sm font-medium text-gray-700 mb-1">Email</label>
            <input id="email" v-model="data.email" type="email" autocomplete="email" required
              class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="your@email.com" />
          </div>
          <div>
            <label for="password" class="block text-sm font-medium text-gray-700 mb-1">Пароль</label>
            <input id="password" v-model="data.password" type="password" autocomplete="new-password" required
              class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="••••••••" />
          </div>
          <div>
            <label for="confirmPassword" class="block text-sm font-medium text-gray-700 mb-1">Підтвердіть пароль</label>
            <input id="confirmPassword" v-model="data.confirmPassword" type="password" autocomplete="new-password"
              required
              class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="••••••••" />
          </div>
        </div>

        <div v-if="errorMessage" class="text-red-500 text-sm text-center py-2">
          {{ errorMessage }}
        </div>

        <div>
          <button type="submit" :disabled="isLoading"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 transition-colors duration-200"
            :class="{ 'opacity-50 cursor-not-allowed': isLoading }">
            <span v-if="!isLoading">Зареєструватися</span>
            <span v-else class="flex items-center">
              <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none"
                viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor"
                  d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z">
                </path>
              </svg>
              Обробка...
            </span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>