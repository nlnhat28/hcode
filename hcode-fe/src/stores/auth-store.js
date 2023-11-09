import { defineStore } from "pinia";
import { cloneDeep } from "lodash";
import storeKey from "./store-key.js";

const useAuthStore = defineStore("auth", {
    state: () => ({
        auth: {},
    }),
    actions: {
        setAuth(newAuth) {
            this.auth = cloneDeep(newAuth);
        },
    },
    persist: true,
});

export default useAuthStore;
