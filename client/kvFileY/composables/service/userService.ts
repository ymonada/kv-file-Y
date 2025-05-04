import type {UserDto} from "~/types/user";
import {API_CONFIG, getApiUrl} from "@/config/api";
import type { ApiResponse } from "~/types/apiRespose";

export async function fetchProfile(): Promise<ApiResponse<UserDto>> {
    try {
        const response = await $fetch<UserDto>(
            `${getApiUrl(API_CONFIG.ENDPOINTS.USER.GET_USER)}`, {
                credentials: "include",
                mode: 'cors',
                headers: process.server ? useRequestHeaders(['cookie']) : {},
            });
        return {
            data: response || {
                id: 0,
                username: "",
                email: ""
            },
            status: 200
        };
    } catch (err:any) {
        console.error("Failed to fetch profile:", err);
        return {
            data: {
                id: 0,
                username: "",
                email: ""
            },
            status: 500,
            error: err.message || 'Failed to fetch profile'
        };
    }
}

