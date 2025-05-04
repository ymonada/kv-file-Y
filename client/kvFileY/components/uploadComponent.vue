<template>
   <div class="flex flex-col items-center justify-center w-full p-4 m-4">
    <label for="dropzone-file" class="flex flex-col items-center justify-center w-1/2 h-64 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 dark:hover:bg-gray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600"
      @dragover.prevent
      @drop.prevent="handleDrop">
        <div class="flex flex-col items-center justify-center pt-5 pb-6">
            <svg class="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 16">
                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2"/>
            </svg>
            <p class="mb-2 text-sm text-gray-500 dark:text-gray-400"><span class="font-semibold">Click to upload</span> or drag and drop</p>
            <p class="text-xs text-gray-500 dark:text-gray-400">SVG, PNG, JPG or GIF (MAX. 800x400px)</p>
        </div>
        <input
          id="dropzone-file"
          type="file"
          @change="handleFileChange"
          multiple
          class="hidden"
          ref="fileInput"
        />
    </label>
  </div>
  <div class="flex flex-col items-center justify-center w-full p-4">
        <div v-if="selectedFiles.length > 0" class="w-2/3 space-y-2">
      <div
        v-for="(file, index) in selectedFiles"
        :key="index"
        class="flex justify-between items-center bg-gray-100 rounded p-2"
      >
        <span class="truncate">{{ file.name }} ({{ formatFileSize(file.size) }})</span>
        <button @click="removeFile(index)" class="text-red-500 bg-gray-300 p-1 m-1 rounded-md  hover:underline text-sm">Delete</button>
      </div>
    </div>

    <div class="flex flex-wrap justify-center gap-4 w-full sm:w-2/3">
      <button
        @click="cancel"
        class="text-white bg-red-500 hover:bg-red-600 w-full sm:w-1/3 rounded-md py-4 m-2"
      >
        Exit
      </button>

      <button
        @click="uploadFiles"
        :disabled="isUploading || selectedFiles.length === 0"
        class="text-white bg-green-500 hover:bg-green-600 w-full sm:w-1/3 rounded-md py-4 m-2"
      >
        {{ isUploading ? 'Loading...' : 'Load' }}
      </button>
    </div>

    <div v-if="uploadStatus" class="mt-2 text-center w-2/3 bg-gray-100 rounded p-2">
      <p :class="uploadError  ? 'text-red-600' : 'text-green-600'">{{ uploadStatus }}</p>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue';
import { fetchFileUpload } from '@/composables/service/fileService';

const selectedFiles = ref<File[]>([]);
const isUploading = ref(false);
const uploadStatus = ref('');
const uploadError = ref(false);

const fileInput = ref<HTMLInputElement | null>(null);
const emit = defineEmits(['exit', 'upload']);

const cancel = () => {
  selectedFiles.value = [];
  uploadStatus.value = '';
  uploadError.value = false;
  if (fileInput.value) {
    fileInput.value.value = '';
  }
  emit('exit');
};

const handleFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (!target.files) return;

  const newFiles = Array.from(target.files);

  newFiles.forEach(file => {
    if (!selectedFiles.value.some(f => f.name === file.name && f.size === file.size)) {
      selectedFiles.value.push(file);
    }
  });

  target.value = '';
};

const handleDrop = (event: DragEvent) => {
  const files = event.dataTransfer?.files;
  if (!files) return;

  const newFiles = Array.from(files);
  newFiles.forEach(file => {
    if (!selectedFiles.value.some(f => f.name === file.name && f.size === file.size)) {
      selectedFiles.value.push(file);
    }
  });
};

const removeFile = (index: number) => {
  selectedFiles.value.splice(index, 1);
};

const formatFileSize = (bytes: number) => {
  if (bytes === 0) return '0 Bytes';
  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
};

const uploadFiles = async () => {
  if (selectedFiles.value.length === 0) return;

  isUploading.value = true;
  uploadStatus.value = '';
  uploadError.value = false;

  const formData = new FormData();
  selectedFiles.value.forEach(file => {
    formData.append('files', file);
  });

  try {
    const response = await fetchFileUpload(formData);
    if (response.status === 200) {
      uploadStatus.value = 'Successfully uploaded files';
      selectedFiles.value = [];
      if (fileInput.value) fileInput.value.value = '';
    } else {
      throw new Error(response.error || 'Error uploading files');
    }
  } catch (error) {
    console.error('Error uploading files:', error);
    uploadStatus.value = 'Error uploading files';
    uploadError.value = true;
  } finally {
    isUploading.value = false;
  }
  emit('upload');
};
</script>
