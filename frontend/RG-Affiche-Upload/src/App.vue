<script setup lang="ts">
import { useRoute } from 'vue-router'; 
import Navigation from './navigation.vue';

const route = useRoute()

</script>

<template>
  <div>
  <Navigation v-if="route.path !== '/display'" /> 

  
  <h1>Het werkt!</h1>
  <RouterView />
  </div>
</template>