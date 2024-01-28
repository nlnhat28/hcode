import { createRouter, createWebHistory } from "vue-router";
import path from "./path.js";
import Home from "@/views/home/Home.vue";
import Login from "@/views/auth/Login.vue";
import Signup from "@/views/auth/Signup.vue";
import Verify from "@/views/auth/Verify.vue";
import ForgotPassword from "@/views/auth/ForgotPassword.vue";
import ChangePassword from "@/views/auth/ChangePassword.vue";
import ProblemsList from "@/views/problem/list/ProblemsList.vue";
import ProblemDetail from "@/views/problem/detail/ProblemDetail.vue";
import ProblemSubmit from "@/views/problem/submit/ProblemSubmit.vue";
import ContestsList from "@/views/contest/list/ContestList.vue";
import ContestDetail from "@/views/contest/detail/ContestDetail.vue";
import ContestSubmit from "@/views/contest/submit/ContestSubmit.vue";
import cf from "@/helper/common-function.js";

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
        path: path.verify,
        name: "verify",
        components: {
            viewApp: Verify,
        },
    },
    {
        path: path.forgotPassword,
        name: "forgotPassword",
        components: {
            viewApp: ForgotPassword,
        },
    },
    {
        path: path.changePassword,
        name: "changePassword",
        components: {
            viewApp: ChangePassword,
        },
    },
    {
        path: path.problems,
        name: "problems",
        components: {
            view1: ProblemsList,
        },
    },
    {
        path: path.problem,
        name: "problemNew",
        components: {
            viewApp: ProblemDetail,
        },
    },
    {
        path: path.problem + ":id",
        name: "problem",
        components: {
            viewApp: ProblemDetail,
        },
    },
    {
        path: path.problemSubmit + ":id",
        name: "problemSubmit",
        components: {
            viewApp: ProblemSubmit,
        },
    },
    {
        path: path.contests,
        name: "contests",
        components: {
            view1: ContestsList,
        },
    },
    {
        path: path.contest,
        name: "contestNew",
        components: {
            viewApp: ContestDetail,
        },
    },
    {
        path: path.contest + ":contestId" + path.submit + ":problemId",
        name: "contestSubmit",
        components: {
            viewApp: ContestSubmit,
        },
    },
    {
        path: path.contest + ":id",
        name: "contest",
        components: {
            viewApp: ContestDetail,
        },
    },
];
const router = createRouter({
    history: createWebHistory(),
    routes: routes,
});

export default router;
