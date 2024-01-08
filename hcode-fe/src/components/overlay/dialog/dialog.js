import emitter from "tiny-emitter/instance";

const dialog = {
    error: (message, callback, header) => {
        emitter.emit("dialogError", message, callback, header);
    },
    info: (message, callback, header) => {
        emitter.emit("dialogInfo", message, callback, header);
    },
    warn: (message, buttons, header) => {
        emitter.emit("dialogWarn", message, buttons, header);
    },
    confirm: (message, buttons, header) => {
        emitter.emit("dialogConfirm", message, buttons, header);
    },
};

export default dialog;
