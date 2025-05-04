// import { ref } from 'vue';
// import type User from '~/types/user';
// import type LoginDto from '~/types/logindto';
// import type RegisterDto from '~/types/registerdto';



// export const useAuth = () => {
//   const user = ref<User | null>(null);
//   const isAuthenticated = ref(false);
//   const isLoading = ref(false);
//   const error = ref<string | null>(null);

//   const API_URL = 'http://localhost:5170/auth';

//   const login = async (credentials: LoginCredentials) => {
//     try {
//       isLoading.value = true;
//       error.value = null;

//       const response = await fetch(`${API_URL}/login`, {
//         method: 'POST',
//         headers: {
//           'Content-Type': 'application/json',
//         },
//         body: JSON.stringify(credentials),
//         credentials: 'include'
//       });

//       if (!response.ok) {
//         throw new Error('Login failed');
//       }

//       const data = await response.json();
//       user.value = data.user;
//       isAuthenticated.value = true;
//       return true;
//     } catch (err) {
//       error.value = err instanceof Error ? err.message : 'An error occurred';
//       return false;
//     } finally {
//       isLoading.value = false;
//     }
//   };

//   const register = async (credentials: RegisterCredentials) => {
//     try {
//       isLoading.value = true;
//       error.value = null;

//       const response = await fetch(`${API_URL}/register`, {
//         method: 'POST',
//         headers: {
//           'Content-Type': 'application/json',
//         },
//         body: JSON.stringify(credentials),
//         credentials: 'include'
//       });

//       if (!response.ok) {
//         throw new Error('Registration failed');
//       }

//       return true;
//     } catch (err) {
//       error.value = err instanceof Error ? err.message : 'An error occurred';
//       return false;
//     } finally {
//       isLoading.value = false;
//     }
//   };

//   const logout = async () => {
//     try {
//       isLoading.value = true;
//       error.value = null;

//       const response = await fetch(`${API_URL}/logout`, {
//         method: 'POST',
//         credentials: 'include'
//       });

//       if (!response.ok) {
//         throw new Error('Logout failed');
//       }

//       user.value = null;
//       isAuthenticated.value = false;
//       return true;
//     } catch (err) {
//       error.value = err instanceof Error ? err.message : 'An error occurred';
//       return false;
//     } finally {
//       isLoading.value = false;
//     }
//   };

//   const checkAuth = async () => {
//     try {
//       isLoading.value = true;
//       error.value = null;

//       const response = await fetch(`${API_URL}/me`, {
//         credentials: 'include'
//       });

//       if (!response.ok) {
//         throw new Error('Not authenticated');
//       }

//       const data = await response.json();
//       user.value = data.user;
//       isAuthenticated.value = true;
//       return true;
//     } catch (err) {
//       user.value = null;
//       isAuthenticated.value = false;
//       return false;
//     } finally {
//       isLoading.value = false;
//     }
//   };

//   return {
//     user,
//     isAuthenticated,
//     isLoading,
//     error,
//     login,
//     register,
//     logout,
//     checkAuth
//   };
// }; 