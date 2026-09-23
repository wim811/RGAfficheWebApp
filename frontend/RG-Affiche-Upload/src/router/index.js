import { createRouter, createWebHistory } from 'vue-router'

import display from '../display.vue'
import upload from '../upload.vue'

const routes = [
    {
        path: '/',
        redirect: '/upload'
    },
    {
        path: '/upload',
        component: upload 
    },
    {
        path: '/display',
        component: display 
    }
]
const router = createRouter({
    history: createWebHistory(),
    routes
})
export default router