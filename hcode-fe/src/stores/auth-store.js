import { defineStore } from "pinia";
import { cloneDeep } from "lodash";

const useAuthStore = defineStore("auth", {
    state: () => ({
        auth: {},
    }),
    actions: {
        setAuth(newAuth) {
            this.auth = cloneDeep(newAuth);
        },
        setVerifyMode(mode) {
            this.auth.VerifyMode = mode;
        },
    },
    persist: {
        storage: localStorage,
    },
});

export default useAuthStore;
