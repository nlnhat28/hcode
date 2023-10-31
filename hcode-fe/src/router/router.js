import { createRouter, createWebHistory } from "vue-router";
import path from "./path.js";
import Home from "@/views/home/Home.vue";
import Login from "@/views/auth/Login.vue";
import Signup from "@/views/auth/Signup.vue";
import ProblemsList from "@/views/problem/ProblemsList.vue";

const routes = [
    {
        path: path.index,
        name: "index",
        components: {
            viewApp: Home,
        },
    },
    {
        path: path.login,
        name: "login",
        components: {
            viewApp: Login,
        },
    },
    {
        path: path.signup,
        name: "signup",
        components: {
            viewApp: Signup,
        },
    },
    {
        path: path.problems,
        name: "problems",
        components: {
            view1: ProblemsList,
        },
    },
];
const router = createRouter({
    history: createWebHistory(),
    routes: routes,
});

export default router;
