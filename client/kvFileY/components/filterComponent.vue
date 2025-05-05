import { _selected } from '../.nuxt/types/tailwind.config';
<script lang="ts" setup>
const emit = defineEmits(['apply']) // оголошуємо подію
const pagesize = ref<number>(10);
const sortOrder = ref<string>('');
const sortBy = ref<string>('');

const apply = ()=>{
  return emit('apply', pagesize.value, sortBy.value, sortOrder.value);
}
</script>

<template>
  <div class="flex flex-col justify-center items-center bg-slate-50 p-1 m-1 rounded-md w-auto">
    <div class="flex p-1 m-1 borger-2 bg-white rounded-md w-9/12 justify-center items-center shadow-md">
      <h3 class="p-1 m-1">Page Size:</h3>
        <select v-model="pagesize" class="bg-gray-100 rounded-md p-1 m-1">
          <option>10</option>
          <option>15</option>
          <option>30</option>
          <option>60</option>
      </select>
    </div>
    <div class="flex p-1 m-1 borger-2 bg-white rounded-md w-9/12 justify-center items-center shadow-md">
      <h3 class="p-1 m-1">Sort by:</h3>
        <select v-model="sortBy" class="bg-gray-100 rounded-md p-1 m-1">
          <option>Nothing</option>
          <option>Name</option>
          <option>Size</option>
          <option>Date</option>
        </select>
    </div>
    <div v-if="sortBy !== 'Nothing'" class="flex flex-col p-1 m-2 borger-1 bg-white rounded-md w-9/12 justify-center items-center shadow-md">
      <div class="p-1 m-1">
        <input type="radio" id="one" value="asc" class="p-1 m-1" v-model="sortOrder" />
        <label for="one">Order by</label>
      </div>
      <div>
        <input type="radio" id="two" value="desc" class="p-1 m-1" v-model="sortOrder" />
        <label for="two">Order by descending</label>    
      </div>
      </div>
    <button class="bg-blue-500 rounded-sm shadow-md p-1 m-1 text-white w-7/12"
    @click="apply">Apply</button>
  </div>
</template>