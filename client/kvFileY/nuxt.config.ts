export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  modules: ['@unocss/nuxt'],
  nitro: {
    devProxy: {
      '/api': {
        target: 'http://localhost:5170',
        changeOrigin: true,
        prependPath: false
      }
    }
  },
  unocss: {
    shortcuts: {
      'btn': 'px-4 py-2 rounded-lg bg-blue-500 text-white hover:bg-blue-600 transition-colors',
      'card': 'p-4 rounded-lg shadow-md bg-white dark:bg-gray-800',
      'input': 'px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500'
    },
    theme: {
      colors: {
        primary: '#3B82F6',
        secondary: '#6B7280'
      }
    }
  },
  vite: {
    server: {
      https: {
        key: './certificates/localhost+2-key.pem', // Шлях до приватного ключа
        cert: './certificates/localhost+2.pem', // Шлях до сертифіката
      },
    },
  },
})