import { checkAuth } from "~/composables/service/authService";

export default defineNuxtRouteMiddleware(async (to, from) => {
 
    let isAuthenticated = await checkAuth();
    if (!isAuthenticated.data || isAuthenticated.error) {
      console.log('isAuthenticated', isAuthenticated);
      return navigateTo('/login');

    }
  })