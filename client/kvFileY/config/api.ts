export const API_CONFIG = {
  BASE_URL: 'http://localhost:5170',
  ENDPOINTS: {
    AUTH: {
      LOGIN: '/auth/login',
      REGISTER: '/auth/register',
      LOGOUT: '/auth/logout',
      CHECK: '/auth/check',
      ME: '/user'
    },
     FILES: {
      BY_ID: '/filebyid/',
      BY_URL: '/filebyurl/',
      PAGE: '/file/page',
      UPLOAD: '/upload',
      COUNT: '/file/count'
    },
    USER: {
      GET_USER: '/user'
    }
  }
} as const;

export const getApiUrl = (endpoint: string) => `${API_CONFIG.BASE_URL}${endpoint}`; 