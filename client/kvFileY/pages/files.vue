<script setup lang="ts">
import { ref } from 'vue';
import { fetchFiles, fetchFileCount } from '@/composables/service/fileService';
import type { FileYDto } from '@/types/fileYDto';
import FileComponent from '@/components/fileComponent.vue';
import FilterComponent from '~/components/filterComponent.vue';
import UploadComponent from '@/components/uploadComponent.vue';
import { file } from '@primeuix/themes/aura/fileupload';
import type { RefSymbol } from '@vue/reactivity';
import uploadComponent from '@/components/uploadComponent.vue';
import { sortIcon } from '@primeuix/themes/aura/datatable';

definePageMeta({
    middleware: 'auth',
    layout: 'active'
})

const searchTerm = ref<string>('');
const files = ref<FileYDto[]>([]);
const isFileUpload = ref<boolean>(false);
const pageSize = ref<number>(10);
const sortBy = ref<string>('');
const sortOrder = ref<string>('');
const fileCount = ref<number>(0);
const currentPage = ref<number>(1);
const search = async () => {
    currentPage.value = 1;
    files.value = await getFiles(currentPage.value) || [];
    fileCount.value = await getFileCount() || 0;
    
}

const getFiles = async (page: number)=>{
  const response = await fetchFiles(page, pageSize.value, searchTerm.value,sortBy.value, sortOrder.value);
  if(response.status === 200) {
        return response.data || [];
    } else {
        console.error(response.error);
    }
}
const getFileCount = async ()=> {
  const countRes = await fetchFileCount();
  if(countRes.status === 200) {
    return countRes.data || 0;
    console.log(JSON.stringify(fileCount.value));
  } else {
    console.error(countRes.error);
  }
}
const addNewFile = () => {
    isFileUpload.value = true;
}

const cancelUpload = () => {
    isFileUpload.value = false;
}
const updateFiles = async (pS:number = 10, sB:string = 'asc', sO:string='name') =>{
   pageSize.value = pS;
   sortBy.value = sB;
   sortOrder.value = sO;
   await search();
}
const loadMore = async () => {
  currentPage.value++;
  const newFiles = await getFiles(currentPage.value) || [];
  files.value.push(...newFiles); // додає елементи з newFiles у кінець files
};


onMounted(async () => {
    await search();
    JSON.stringify(files?.value);
    
})
const fileCountExceeded = computed(() => {
  return (files.value?.length ?? 0) < fileCount.value;
});
</script>

<template>
    <div class="flex flex-row flex-wrap gap-1 justify-center items-center">
      <div class="flex flex-row flex-wrap text-white w-full sm:w-1/4 md:w-1/6 lg:w-1/8 h-10 bg-indigo-500 rounded-md p-1 m-1 justify-center items-center">
        <h3 class="truncate">Files Count {{ fileCount }}</h3>
      </div>
        <button @click="addNewFile" class="search-button text-white w-full sm:w-1/12 h-10 bg-yellow-500 rounded-md p-1 m-1 hover:bg-yellow-600">Add</button>
        <input type="text" class="search-input rounded-md w-full sm:w-1/2 h-10 p-1 m-1" v-model="searchTerm" placeholder="Search" />
        <button @click="search()" class="search-button text-white w-full sm:w-1/5 h-10 bg-indigo-600 rounded-md p-1 m-1 hover:bg-blue-600">Search</button>
    </div>
    <div v-if="!isFileUpload" class="flex flex-col lg:flex-row justify-center gap-2 p-2">
  <!-- FilterComponent -->
  <div class="w-full lg:w-1/4 mb-2 lg:mb-0">
    <FilterComponent class="w-full" @apply="updateFiles" />
  </div>

  <!-- File grid -->
  <div class="w-full lg:w-3/4">
    <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-3 xl:grid-cols-4 gap-2">
      <div v-for="file in files" :key="file.id">
        <FileComponent
          :file="file"
          class="hover:scale-105 transition-all duration-100 hover:bg-gray-100 rounded-md"
        />
      </div>
    </div>
  </div>
  
</div> 
  
  <div v-if="!isFileUpload && fileCountExceeded" class="flex flex-row justify-center items-center" >
    <button @click="loadMore" class="bg-green-600 text-white rounded-md shadow-md p-2 m-2">Load More</button>
  </div>

  <UploadComponent v-else-if="isFileUpload" @exit="cancelUpload" @upload="search" />
   
</template>