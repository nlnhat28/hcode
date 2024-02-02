import { defineStore } from "pinia";
import { cloneDeep } from "lodash";

const useLanguageStore = defineStore("language", {
    state: () => ({
        languages: [],
    }),
    actions: {
        setLanguages(newLanguages) {
            this.languages = cloneDeep(newLanguages);
        },
    },
    persist: {
        storage: localStorage,
    },
});

export default useLanguageStore;
