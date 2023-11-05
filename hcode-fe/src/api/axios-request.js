import axios from "axios";
import emitter from "tiny-emitter/instance";
import enums from "@/enums/enums.js";
import { t } from "@/i18n/i18n.js";

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
            emitter.emit("dialogError", t("msg.cannotConnectServer"));
        } else {
            const data = error.response.data;

            const userMsg = data.UserMsg;
            const key = data.Data?.Key;
            const errorKey = data.Data?.ErrorKey;

            let message = t("msg.clientError");
            if (userMsg && userMsg != "") {
                message = userMsg;
            }

            let actionOnClose = null;

            switch (errorKey) {
                // Nếu exception key là form item thì focus vào form item bị lỗi
                case enums.errorKey.formItem:
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

            emitter.emit("dialogError", message, actionOnClose);
        }
    }
);

export default axiosRequest;
