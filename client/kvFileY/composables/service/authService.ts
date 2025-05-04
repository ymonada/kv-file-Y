import { API_CONFIG, getApiUrl } from "~/config/api";
import type { LoginDto } from "~/types/logindto";
import type { RegisterDto } from "~/types/registerdto";
import type { ApiResponse } from "~/types/apiRespose";

export async function loginFetch(loginDto: LoginDto): Promise<ApiResponse<boolean>> {
  try {
    const response = await $fetch<{ token?: string }>(
      `${getApiUrl(API_CONFIG.ENDPOINTS.AUTH.LOGIN)}`,
      {
        method: 'POST',
        body: loginDto,
        credentials: 'include',
      }
    );

    return {
      data: response.token !== undefined,
      status: 200
    };
  } catch (error: any) {
    return {
      error: error.data?.message || 'Login failed',
      status: error.status || 500
    };
  }
}

export async function registerFetch(regDto: RegisterDto): Promise<ApiResponse<boolean>> {
  try {
    const response = await $fetch<{ id: string, email: string }>(
      `${getApiUrl(API_CONFIG.ENDPOINTS.AUTH.REGISTER)}`,
      {
        method: 'POST',
        body: regDto,
        credentials: 'include',
      }
    );

    return {
      data: response.id !== undefined,
      status: 201
    };
  } catch (error: any) {
    return {
      error: error.data?.message || 'Registration failed',
      status: error.status || 500
    };
  }
}

export async function fetchLogout(): Promise<ApiResponse<void>> {
  try {
    await $fetch(
      `${getApiUrl(API_CONFIG.ENDPOINTS.AUTH.LOGOUT)}`,
      {
        method: 'POST',
        credentials: 'include',
      }
    );

    return {
      status: 200
    };
  } catch (error: any) {
    return {
      error: error.data?.message || 'Logout failed',
      status: error.status || 500
    };
  }
}


export async function checkAuth(): Promise<ApiResponse<boolean>> {
  try {
    const { data, error } = await useFetch<boolean>(
      `${getApiUrl(API_CONFIG.ENDPOINTS.AUTH.CHECK)}`,
      {
        credentials: 'include',
        mode: 'cors',
        headers: process.server ? useRequestHeaders(['cookie']) : {}
      }
    );

    return {
      data: data.value || false,
      status: data.value ? 200 : 401
    };
  } catch (error: any) {
    return {
      data: false, 
      error: error.data?.message || 'Auth check failed',
      status: error.status || 500
    };
  }
}
