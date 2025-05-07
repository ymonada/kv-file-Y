<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import type { UserDto } from "@/types/user";
import { fetchProfile } from '@/composables/service/userService';
import { useRouter } from 'vue-router';

definePageMeta({
  middleware: 'auth',
  layout: 'active',
}); 

const router = useRouter();
const isLoading = ref<boolean>(true);
const profile = ref<UserDto | null>(null);

onMounted(async () => {
  try {
    const response = await fetchProfile();
    console.log('API Response:', response);
    
    if (response.status === 200 && response.data) {
      profile.value = response.data;
    } else {
      console.error('Error fetching profile:', response.error);
      profile.value = null;
    }
  } catch (error) {
    console.error('Unexpected error:', error);
    profile.value = null;
  } finally {
    isLoading.value = false;
  } 
  
});
</script>

<template>
  <div v-if="isLoading" class="loading">
    <h1>Loading...</h1>
  </div>
  <div v-else-if="!profile" class="error">
    <h1>Failed to load profile</h1>
    <button @click="router.push('/login')">Login</button>
  </div>
  <div v-else class="profile">
    <h1>Profile</h1>
	<p>Id: {{ profile.id }}</p>
    <p>Name: {{ profile.username }}</p>
    <p>Email: {{ profile.email }}</p>
  </div>
</template>
