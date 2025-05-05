<script setup lang="ts">
import type { FileYDto } from '@/types/fileYDto';
import { getApiUrl, API_CONFIG } from '@/config/api';
import { fetchFileDownload } from '@/composables/service/fileService';
import type { ApiResponse } from '@/types/apiRespose';

const props = defineProps<{
  file: FileYDto;
}>();


const fileUrl = computed(() => {
  return `${getApiUrl(API_CONFIG.ENDPOINTS.FILES.BY_URL)}${props.file.filePath.replaceAll('/', '%2F')}`;
});
const isImage = ()=>{
  console.log(fileUrl.value);
  return props.file.contentType.startsWith('image/');
} 
const getFileIcon = (type: string) => {
  if (type.startsWith('image/')) return '🖼️';
  if (type.includes('pdf')) return '📕';
  if (type.includes('word')) return '📄';
  if (type.includes('excel')) return '📊';
  return '📁';
};

const showInfo = () => {
  console.log('showInfo');
}
const downloadFile = async () => {
  const response =  await fetchFileDownload(props.file.filePath.replaceAll('/', '%2F'));
  if(response.status === 200) {
    // Create a temporary anchor element
    
  } else {
    console.error(response.error);
  }
} 

</script>
<template>
  <div class="w-auto flex justify-center items-center bg-white shadow-md rounded-md p-2 m-2 border-2 border-gray-300">
    <div class="flex flex-col justify-center items-center w-full">
      <div class="w-28 h-28 flex items-center justify-center p-1">
        <div v-if="isImage()" class="w-full h-full flex items-center justify-center">
          <img :src="fileUrl" alt="File" class="w-full h-full object-cover rounded-lg border border-gray-200 shadow" />
        </div>
        <div v-else class="text-4xl">
          {{ getFileIcon(file.contentType) }}
        </div>
      </div>

      <!-- Назва файлу + кнопки -->
      <div class="bg-gray-200 rounded-md flex  items-center w-full px-2 py-1 mt-2">
        <span class="truncate flex-grow items-center text-sm sm:text-base" :title="file.fileName">
          {{ file.fileName }}
        </span>
        <button @click="showInfo" class="p-1 hover:bg-gray-100 rounded-full">
          <img src="/info.svg" alt="Info" class="w-5 h-5" />
        </button>
        <button @click="downloadFile" class="p-1 hover:bg-gray-100 rounded-full">
          <img src="/download.svg" alt="Download" class="w-5 h-5" />
        </button>
      </div>
    </div>
  </div>
</template>
