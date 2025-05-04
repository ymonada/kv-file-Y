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
  <div class="flex justify-center items-center bg-white shadow-md rounded-md p-1 m-1 border-2 borger-gray-700">
    <div class="flex flex-col justify-center items-center">
      <div class="w-32 h-32 flex items-center justify-center p-1 ">
        <div v-if="isImage()" class="w-full h-full flex items-center justify-center">
          <img :src="fileUrl" alt="File" class="w-full h-full object-cover rounded-lg border-2 border-gray-100 shadow-md" />
        </div>
        <div v-else class="text-4xl">
          {{ getFileIcon(file.contentType) }}
        </div>
      </div>
      <div class="flex flex-row justify-center items-center bg-gray-200 rounded-md w-auto">
        <h3 class="text-lg bg-gray-100 rounded-md p-1 m-1 w-auto">{{ file.fileName }}</h3>
        <button @click=showInfo class="p-2 hover:bg-gray-100 rounded-full">
          <img src="/info.svg" alt="Copy" class="w-6 h-6" />
        </button>
        <button @click=downloadFile class="p-2 hover:bg-gray-100 rounded-full">
          <img src="/download.svg" alt="Download" class="w-6 h-6" />
        </button>
      </div>
    </div>
  </div>
</template>