import axios from "axios";
import emitter from "tiny-emitter/instance";
import { t } from "@/i18n/i18n.js";

const axiosRequest = axios.create({
    baseURL: window.config.server.baseURL,
    headers: {
        "Content-Type": "application/json"
    },
});
/**
 * Set up Axios interceptor for request
 */
axiosRequest.interceptors.request.use(
    (config) => {
        const token = JSON.parse(localStorage.getItem("__persisted__account"))?.account?.Token;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);
/**
 * Add a response interceptor
 *
 * Author: nlnhat (25/06/2023)
 */
axiosRequest.interceptors.response.use(
    (response) => {
        return response.data;
    },
    (error) => {
        if (!error.response?.status) {
            emitter.emit("dialogError", t("msg.cannotConnectServer"));
        } else {
            const data = error.response?.data;
            let userMsg = data.UserMsg;

            if (userMsg == null || userMsg === "") {
                userMsg = t("msg.clientError");
            }

            emitter.emit("dialogError", userMsg);
        }
    }
);

export default axiosRequest;
