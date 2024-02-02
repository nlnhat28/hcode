import { defineStore } from "pinia";
import { cloneDeep } from "lodash";

const useContestStore = defineStore("contest", {
    state: () => ({
        contest: {},
    }),
    actions: {
        setContest(newContest) {
            this.contest = cloneDeep(newContest);
        },
    },
    persist: {
        storage: sessionStorage,
    },
});

export default useContestStore;
