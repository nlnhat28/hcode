<template>
    <div class="contest-problem-item">
        <div class="contest-problem__order flex-center">
            {{ instance.Order }}
        </div>
        <div class="flex align-center justify-between col-gap-12 flex-1">
            <div class="flex align-center justify-between col-gap-16 flex-1">
                <div class="contest-problem__name flex-center dark">
                    <v-combobox
                        v-model="selectedProblem"
                        optionLabel="ProblemName"
                        editable
                        showClear
                        :placeholder="$t('problem.problem')"
                        :options="problems"
                        @change="onChangeProblem"
                    ></v-combobox>
                </div>
                <div
                    class="contest-problem__difficulty flex dark"
                    style="width: 60px;"
                >
                    <div
                        :style="{
                            color: $cv.difficultyToColor(selectedProblem.Difficulty),
                            fontWeight: 700
                        }"
                        v-if="selectedProblem"
                    >
                        {{ $cv.enumToResource(selectedProblem.Difficulty, $enums.difficulty) }}
                    </div>
                </div>
            </div>
            <div class="contest-problem__score flex-center dark">
                <v-input-text
                    v-model="instance.Score"
                    type='number'
                    :dataInput="$enums.dataInput.integer"
                    :minValue="0"
                    :label="$t('contest.score')"
                >
                </v-input-text>
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
import { validate } from "@/mixins/mixins";
import problemEnum from "@/enums/problem-enum";
import contestEnum from "@/enums/contest-enum";
import { handleError } from "vue";

export default {
    name: 'ContestProblemItem',
    mixins: [validate],
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
        /**
         * Danh sách contest problem khác
         */
        contestProblems: {
            type: Array,
            default: []
        },
        /**
         * Danh sách problem
         */
        problems: {
            type: Array,
            default: []
        }
    },
    data() {
        return {
            instance: {},
            selectedProblem: null,
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
        problems: {
            handler() {
                if (this.problems) {
                    const problem = this.problems.find(p => p.ProblemId == this.instance.ProblemId)
                    this.selectedProblem = problem;
                };
            },
            deep: false,
        }
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

            if (this.problems) {
                const problem = this.problems.find(p => p.ProblemId == this.instance.ProblemId)
                this.selectedProblem = problem;
            };
        },
        /**
         * Click xoá
         */
        clickDelete() {
            this.$emit('onDelete', this.contestProblem);
        },
        /**
         * Thay đổi problem
         */
        onChangeProblem() {
            if (this.selectedProblem != null && typeof(this.selectedProblem) == 'object') {
                this.instance.ProblemId = this.selectedProblem.ProblemId;
            }
            else {
                this.instance.ProblemId = null;
            }
        }
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
}

.contest-problem-item:focus-within {
    background-color: var(--dark-300);
}

.contest-problem-item:hover {
    background-color: var(--dark-200);
}

.contest-problem__order,
.contest-problem__function {
    width: 56px;
    height: 100%;
}

.contest-problem__order {
    font-weight: 700;
    color: var(--grey-400)
}

.contest-problem__name {
    flex: 1;
}

.contest-problem__score {
    width: 120px;
    height: 100%;
}
</style>