export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  modules: ['@nuxtjs/tailwindcss'],
  runtimeConfig: {
    public: {
      apiUrl: 'http://localhost:5170'
    }
  },
  nitro: {
    devProxy: {
      '/api': {
        target: 'http://localhost:5170',
        changeOrigin: true,
        prependPath: false
      }
    }
  },
  css: ['~/assets/css/tailwind.css'],
  
  tailwindcss: {
    exposeConfig: true,
    viewer: true,
    // and more...
  },
 postcss: {
    plugins: {
      autoprefixer: {},
    },
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