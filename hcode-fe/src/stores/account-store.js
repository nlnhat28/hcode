import { defineStore } from "pinia";
import { cloneDeep } from "lodash";

const useAccountStore = defineStore("account", {
    state: () => ({
        account: {},
    }),
    actions: {
        setAccount(newAccount) {
            this.account = cloneDeep(newAccount);
        },
    },
    persist: {
        storage: localStorage,
    },
});

export default useAccountStore;
