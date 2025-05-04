<script setup lang="ts">
import type { LoginDto } from "~/types/logindto";
import { loginFetch } from "~/composables/service/authService";

const router = useRouter();
const data = ref<LoginDto>({ email: "", password: "" });
const errorMessage = ref<string>("");
const isLoading = ref<boolean>(false);

const handleSubmit = async (e: Event) => {
    e.preventDefault();
    errorMessage.value = "";

    // Валідація
    if (!data.value.email || !data.value.password) {
        errorMessage.value = "Будь ласка, заповніть всі поля";
        return;
    }

    if (!/^\S+@\S+\.\S+$/.test(data.value.email)) {
        errorMessage.value = "Будь ласка, введіть коректний email";
        return;
    }

    try {
        isLoading.value = true;
        const { error } = await loginFetch(data.value);

        if (error) {
            errorMessage.value = error;
            return;
        }

        await router.push("/files");
    } catch (err) {
        errorMessage.value = "Сталася помилка при вході";
        console.error("Login error:", err);
    } finally {
        isLoading.value = false;
    }
};
</script>

<template>
    <div
        class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8"
    >
        <div
            class="max-w-md w-full space-y-8 bg-white p-8 rounded-lg shadow-md"
        >
            <div class="text-center">
                <h1 class="text-3xl font-extrabold text-gray-900">
                    Вхід в акаунт
                </h1>
                <p class="mt-2 text-sm text-gray-600">
                    Ще не маєте акаунту?
                    <NuxtLink
                        to="/register"
                        class="font-medium text-indigo-600 hover:text-indigo-500"
                    >
                        Зареєструватися
                    </NuxtLink>
                </p>
            </div>

            <form class="mt-8 space-y-6" @submit="handleSubmit">
                <div class=" rounded-md shadow-sm space-y-4">
                    <div>
                        <label
                            for="email"
                            class="block text-sm font-medium text-gray-700 mb-1"
                            >Email</label
                        >
                        <input
                            id="email"
                            v-model="data.email"
                            type="email"
                            autocomplete="email"
                            required
                            class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            placeholder="your@email.com"
                        />
                    </div>
                    <div>
                        <label
                            for="password"
                            class="block text-sm font-medium text-gray-700 mb-1"
                            >Пароль</label
                        >
                        <input
                            id="password"
                            v-model="data.password"
                            type="password"
                            autocomplete="current-password"
                            required
                            class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            placeholder="••••••••"
                        />
                    </div>
                </div>

                <div
                    v-if="errorMessage"
                    class="text-red-500 text-sm text-center py-2"
                >
                    {{ errorMessage }}
                </div>

                <div>
                    <button
                        type="submit"
                        :disabled="isLoading"
                        class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 transition-colors duration-200"
                        :class="{ 'opacity-50 cursor-not-allowed': isLoading }"
                    >
                        <span v-if="!isLoading">Увійти</span>
                        <span v-else class="flex items-center">
                            <svg
                                class="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                            >
                                <circle
                                    class="opacity-25"
                                    cx="12"
                                    cy="12"
                                    r="10"
                                    stroke="currentColor"
                                    stroke-width="4"
                                ></circle>
                                <path
                                    class="opacity-75"
                                    fill="currentColor"
                                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                                ></path>
                            </svg>
                            Обробка...
                        </span>
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>
