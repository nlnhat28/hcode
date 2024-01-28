<template>
    <div class="contest-problem-item">
        <div class="flex align-center justify-between col-gap-12 flex-1">
            <div class="contest-problem__name flex-center">
                {{ nameComputed }}
            </div>
        </div>
        <div class="contest-problem__function flex-center">
            <v-icon
                icon="far fa-circle-xmark"
                color="danger"
                applyPointer
                :title="$t('com.delete')"
                @click="clickDelete"
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
    },
    data() {
        return {
            instance: {},
        }
    },
    emits: ['onDelete'],
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
        nameComputed() {
            let name = `${index + 1}. ${this.$t('problem.problem')}`
            if (this.instance && this.instance.Score) {
                name += `${this.instance.Score} ${this.$t('contest.score')}}`
            }
        }
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
         * Click xoá
         */
        clickDelete() {
            this.$emit('onDelete', this.contestProblem);
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

.contest-problem__name {
    font-weight: 700;
    color: var(--grey-400);
    padding-left: 12px;
}

.contest-problem__score {
    width: 120px;
    height: 100%;
}

.contest-problem__function {
    width: 56px;
    height: 100%;
}
</style>