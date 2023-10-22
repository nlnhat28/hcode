import emitter from "tiny-emitter/instance";

const toast = {
    success: (message) => {
        emitter.emit("toastSuccess", message);
    },
    error: (message) => {
        emitter.emit("toastError", message);
    },
    warn: (message) => {
        emitter.emit("toastWarn", message);
    },
    info: (message) => {
        emitter.emit("toastInfo", message);
    },
};

export default toast;
