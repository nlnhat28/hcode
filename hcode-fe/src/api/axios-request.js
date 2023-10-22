import axios from "axios";
import emitter from "tiny-emitter/instance";
import resources from "@/constants/resources.js";
import enums from "@/constants/enums.js";

const axiosRequest = axios.create({
    baseURL: window.config.server.baseURL,
    headers: {
        "Content-Type": "application/json",
    },
    timeout: window.config.server.timeout,
});
/**
 * Add a response interceptor
 *
 * Author: nlnhat (25/06/2023)
 */
axiosRequest.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (!error.response?.status) {
            emitter.emit(
                "showDialogError",
                resources["vn"].cannotConnectServer
            );
        } else {
            const data = error.response.data;

            const userMsg = data.UserMsg;
            const key = data.Data?.Key;
            const exceptionKey = data.Data?.ExceptionKey;

            let message = resources["vn"].userMsg400;
            if (userMsg && userMsg != "") message = userMsg;

            let actionOnClose = null;

            switch (exceptionKey) {
                // Nếu exception key là form item thì focus vào form item bị lỗi
                case enums.exceptionKey.formItem:
                    let ref = null;
                    if (key && key != "") ref = key;

                    emitter.emit("setMessageFormItem", ref, message);

                    actionOnClose = () => {
                        emitter.emit("focusFormItem", ref);
                    };
                    break;

                default:
                    break;
            }

            emitter.emit("showDialogError", message, actionOnClose);
        }
    }
);

export default axiosRequest;
