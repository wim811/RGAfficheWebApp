<template>

  <div>

    <navigation />
    <display />
    <div>

      <hr>
      <h1>Upload een Affiche</h1>
      <hr>

      <label for="file-input" class="btn">
        Upload File
      </label>

      <input
        id="file-input"
        type="file"
        :multiple="multiple"
        :accept="accept"
        @change="handleFileSelect"
      />

      <br>

      <p v-if="errorMessage" class="error-text">
        {{ errorMessage }}
      </p>

      <ul v-if="files.length" class="file-list">

        <li
          v-for="(file, index) in files"
          :key="`${file.name}-${index}`"
        >

          <img
            v-if="file.type.startsWith('image/')"
            :src="getPreviewUrl(file)"
            class="file-preview"
          />

          <span>
            {{ file.name }}
            ({{ (file.size / 1024).toFixed(1) }} KB)
          </span>

          <button @click="removeFile(index)">
            Remove
          </button>

        </li>

      </ul>

      <br>

      <button
        v-if="files.length"
        @click="uploadFile"
        :disabled="uploading"
      >
        {{ uploading ? 'Uploaden...' : 'Upload affiche' }}
      </button>

      <p v-if="uploadMessage" class="success-text">
        {{ uploadMessage }}
      </p>

    </div>

  </div>

</template>


<script setup lang="ts">

import { ref } from 'vue'
import navigation from "./navigation.vue"
import display from './display.vue'




const props = withDefaults(defineProps<{
  multiple?: boolean
  maxSizeMB?: number
  accept?: string
}>(), {

  multiple: false,
  maxSizeMB: 2000,
  accept: 'image/*,application/pdf'

})


const files = ref<File[]>([])

const errorMessage = ref<string | null>(null)

const uploadMessage = ref<string | null>(null)

const uploading = ref(false)


function handleFileSelect(e: Event) {

  errorMessage.value = null
  uploadMessage.value = null

  const input = e.target as HTMLInputElement

  const selectedFiles = Array.from(input.files || [])

  const validFiles = selectedFiles.filter(file => {

    const sizeMB = file.size / (1024 * 1024)

    if (sizeMB > props.maxSizeMB) {

      errorMessage.value =
        `File "${file.name}" is groter dan ${props.maxSizeMB} MB.`

      return false

    }

    return true

  })

  files.value = files.value.concat(validFiles)

}


function removeFile(index: number) {

  files.value.splice(index, 1)

}


function getPreviewUrl(file: File): string {

  return URL.createObjectURL(file)

}


async function uploadFile() {

  if (files.value.length === 0) {

    errorMessage.value = 'Selecteer eerst een affiche.'

    return

  }

  uploading.value = true

  errorMessage.value = null
  uploadMessage.value = null


  try {

    const formData = new FormData()

    const file = files.value[0]

    formData.append('file', file)


    const response = await fetch(
      'http://localhost:5062/api/Posters/upload',
      {
        method: 'POST',
        body: formData
      }
    )


    if (!response.ok) {

      throw new Error(
        `Upload mislukt (${response.status})`
      )

    }


    const result = await response.json()

    console.log(result)


    uploadMessage.value =
      'Affiche succesvol opgeslagen!'


    files.value = []


  } catch (error) {

    console.error(error)

    errorMessage.value =
      'Er is iets fout gegaan bij het uploaden.'

  } finally {

    uploading.value = false

  }

}

</script>