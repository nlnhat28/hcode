import { createRouter, createWebHistory } from "vue-router";
import path from "./path.js"
import Home from "@/views/home/Home.vue";
import Auth from "@/views/auth/Auth.vue";

const routes = [
    {
        path: path.index,
        component: Home,
    },
    {
        path: path.login,
        component: Auth,
    },
    {
        path: path.signup,
        component: Auth,
    },
];
const router = createRouter({
    history: createWebHistory(),
    routes: routes,
});

export default router;
