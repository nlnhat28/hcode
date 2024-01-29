<template>
    <div :class="['contest-problem-item', {'selected': isSelected}]" @click="selectItem">
        <div class="flex align-center col-gap-12 flex-1">
            <div :class="['contest-problem__name']">
                {{ `${index + 1}. ${$t('problem.problem')} ${index + 1}` }}
            </div>
            <div class="contest-problem__score">
                {{ `(${instance.Score != null ? instance.Score : 0} ${$t('contest.score')})` }}
            </div>
        </div>
        <div class="contest-problem__function flex-center">
            <v-icon
                :icon="$cv.problemAccountStateToIcon(instance.ContestProblemAccountState)"
                :color="$cv.problemAccountStateToColor(instance.ContestProblemAccountState)"
                :title="$cv.enumToResource(instance.ContestProblemAccountState, problemEnum.problemAccountState)"
            />
        </div>
    </div>
</template>
<script>
import problemEnum from "@/enums/problem-enum";
import contestEnum from "@/enums/contest-enum";
import { handleError } from "vue";

export default {
    name: 'ContestProblemItem',
    props: {
        /**
         * Index
         */
        index: {
            type: Number,
        },
        /**
         * Thông tin contest problem
         */
        contestProblem: {
            type: Object,
            default: {}
        },
        /** Đang được chọn */
        isSelected: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            instance: {},
            problemEnum: problemEnum,
        }
    },
    emits: ['selected'],
    created() {
        this.assignInstance();
    },
    mounted() {
        this.refs = [this.$refs['refContestProblemName']];
    },
    watch: {
        contestProblem() {
            this.assignInstance();
        },
        index() {
            this.instance.Order = this.index + 1;
        },
    },
    computed: {
    },
    methods: {
        /**
         * Gán instance
         */
        assignInstance() {
            this.instance = this.contestProblem;
            this.instance.Order = this.index + 1;
        },
        /**
         * Click chọn
         */
        selectItem() {
            this.$emit('selected', this.instance);
        },
    }
}
</script>
<style scoped>
.contest-problem-item {
    width: 100%;
    height: 60px;
    min-height: 60px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    background-color: var(--dark-400);
    color: var(--grey-400);
    border-radius: 8px;
    transition: background-color 0.2s;
    cursor: pointer;
}

.contest-problem-item:focus-within {
    background-color: var(--dark-300);
}

.contest-problem-item:hover {
    background-color: var(--dark-200);
}
.contest-problem-item.selected {
    color: var(--orange-400);
    /* background-color: var(--dark-100); */
}
.contest-problem__name {
    font-weight: 700;
    width: 130px;
    padding-left: 20px;
}
.contest-problem__score {
    /* font-weight: 700; */
    height: 100%;
}

.contest-problem__function {
    width: 60px;
    height: 100%;
}
</style>