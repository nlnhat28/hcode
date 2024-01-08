import { defineStore } from "pinia";
import { cloneDeep } from "lodash";

const useProblemStore = defineStore("problem", {
    state: () => ({
        problem: {},
    }),
    actions: {
        setProblem(newProblem) {
            this.problem = cloneDeep(newProblem);
        },
    },
    persist: {
        storage: sessionStorage,
    },
});

export default useProblemStore;
