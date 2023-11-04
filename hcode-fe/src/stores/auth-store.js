import { defineStore } from "pinia";
import { cloneDeep } from "lodash";

const useAuthStore = defineStore("auth", {
    state: () => ({
        auth: {}
    }),
    actions: {
        setAuth(newAuth) {
            this.auth = cloneDeep(newAuth);
        }
    },
});

export default useAuthStore;
