<script setup lang="ts">
import { fetchLogout } from '@/composables/service/authService';


const logout = async () => {
  const response = await fetchLogout();
  if(response.status === 200) {
    await navigateTo({ path: '/login' })
  } else {
    console.error(response.error);
  }
}

const profile = () => {
  navigateTo({ path: '/profile' })
}

const files = () => {
  navigateTo({ path: '/files' })
}

</script>

<template>
  <div class="min-h-screen flex flex-col">
    <div class="flex flex-row justify-center p-4">
      <button @click="profile" class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 p-2 m-2">Profile</button>
      <button @click="files" class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 p-2 m-2">Files</button>
      <button @click="logout" class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 p-2 m-2">Logout</button>
    </div>
    
    <main class="flex-grow">
      <slot />
    </main>
  </div>
</template>

<style scoped>

</style>