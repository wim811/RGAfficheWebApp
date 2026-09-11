<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

const posters = ref([])
const currentIndex = ref(0)

let timer = null

const API_URL = 'http://localhost:5062'

async function loadPosters() {
  try {
    const response = await fetch(`${API_URL}/api/Posters`)

    if (!response.ok) {
      throw new Error('Affiches konden niet worden opgehaald.')
    }

    posters.value = await response.json()

    console.log('Affiches opgehaald:', posters.value)

    if (posters.value.length > 0) {
      startSlideshow()
    }
  } catch (error) {
    console.error('Fout bij ophalen affiches:', error)
  }
}

function startSlideshow() {
  timer = setInterval(() => {
    currentIndex.value++

    if (currentIndex.value >= posters.value.length) {
      currentIndex.value = 0
    }
  }, 15000)
}

onMounted(() => {
  loadPosters()
})

onUnmounted(() => {
  if (timer) {
    clearInterval(timer)
  }
})
</script>

<template>
  <div class="display">

    <div v-if="posters.length === 0" class="message">
      Geen affiches beschikbaar.
    </div>

    <img
      v-else
      :src="`${API_URL}/api/Posters/${posters[currentIndex].id}/image`"
      :alt="posters[currentIndex].fileName"
      class="poster"
    />

  </div>
</template>

<style scoped>
.display {
  width: 100vw;
  height: 100vh;

  background-color: black;

  display: flex;
  justify-content: center;
  align-items: center;

  overflow: hidden;
}

.poster {
  max-width: 100%;
  max-height: 100%;

  object-fit: contain;
}

.message {
  color: white;
  font-size: 2rem;
}
</style>