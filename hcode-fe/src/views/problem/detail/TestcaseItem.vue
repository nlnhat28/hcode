<template>
    <div class="testcase-item">
        <div
            class="testcase__panel"
            @click="clickPanel"
        >
            <div class="flex-align-center col-gap-16">
                <div class="testcase__label flex-center">
                    {{ index + 1 }}. Testcase {{ index + 1 }}
                </div>
                <!-- Status -->
                <v-button
                    v-if="isShowStatusButton"
                    style="padding: 0.25rem 0.5rem"
                    outlined
                    :severity="severityStatusButton"
                    :label="instance.Status.status_name"
                    @click="clickStatus"
                ></v-button>
            </div>
            <div class="testcase__function flex-align-center col-gap-12">
                <v-icon
                    applyPointer
                    :icon="this.instance.AllowView ? 'far fa-lock-keyhole-open' : 'far fa-lock-keyhole'"
                    :color="this.instance.AllowView ? 'light' : 'warn'"
                    :title="$t('problem.showOrHideTestcase')"
                    @click="clickLock"
                />
                <v-icon
                    icon="far fa-circle-xmark"
                    color="danger"
                    applyPointer
                    :title="$t('com.delete')"
                    @click="clickDelete"
                />
            </div>
        </div>
        <!-- Chi tiết -->
        <div
            class="testcase__detail flex-column row-gap-20"
            v-if="isShowDetail"
        >
            <div
                class="flex-column row-gap-12"
                v-if="!$cf.isEmptyArray(parameters)"
            >
                <div
                    class="testcase__input"
                    v-for="(param, index) in parameters"
                    :key="index"
                >
                    <div class="testcase__parameter-name">
                        {{ param.ParameterName || `${$t('problem.parameter')} ${param.ParameterOrder}` }}
                    </div>
                    <div class="testcase__parameter-input">
                        <v-input-text
                            hasClear
                            v-model="instance.Inputs[index]"
                        ></v-input-text>
                    </div>
                </div>
            </div>
            <div class="testcase__input">
                <div class="testcase__parameter-name">
                    {{ $t('problem.expectedOutput') }}
                </div>
                <div class="testcase__parameter-input">
                    <v-input-text
                        hasClear
                        v-model="instance.ExpectedOutput"
                    ></v-input-text>
                </div>
            </div>
            <div class="flex-center btn-hide-detail">
                <v-button
                    icon="far fa-circle-chevron-up"
                    severity="warning"
                    text
                    raised
                    rounded
                    @click="this.isShowDetail = false"
                />
            </div>
        </div>
        <div
            class="testcase__detail flex-column row-gap-20"
            v-if="isShowStatus"
        >
            <div
                class="flex-column row-gap-12"
                v-if="isShowWrongAnswerDetail"
            >
                <div class="testcase__input">
                    <div class="testcase__parameter-name">
                        {{ $t('problem.expectedOutput') }}
                    </div>
                    <div class="testcase__parameter-input">
                        <v-input-text
                            isReadOnly
                            v-model="instance.ExpectedOutput"
                        ></v-input-text>
                    </div>
                </div>
                <div class="testcase__input">
                    <div class="testcase__parameter-name">
                        {{ $t('problem.output') }}
                    </div>
                    <div class="testcase__parameter-input">
                        <v-input-text
                            isReadOnly
                            hasCopy
                            v-model="logStatus"
                        ></v-input-text>
                    </div>
                </div>
            </div>
            <!-- Status -->
            <v-log-view
                v-else
                :content="logStatus"
            >
            </v-log-view>
        </div>
    </div>
</template>
<script>
import { validate } from "@/mixins/mixins";
import problemEnum from "@/enums/problem-enum";

export default {
    name: 'TestcaseItem',
    mixins: [validate],
    props: {
        /**
         * Index
         */
        index: {
            type: Number,
        },
        /**
         * Thông tin testcase
         */
        testcase: {
            type: Object,
            default: {}
        },
        /**
         * Danh sách testcase khác
         */
        testcases: {
            type: Array,
            default: []
        },
        /**
         * Danh sách parameters
         */
        parameters: {
            type: Array,
            default: []
        }
    },
    data() {
        return {
            instance: {},
            /**
             * Hiện chi tiết
             */
            isShowDetail: false,
            /**
             * Hiện log
             */
            isShowStatus: false,
            problemEnum: problemEnum,
        }
    },
    emits: ['onDelete'],
    created() {
        this.instance = this.testcase;

        this.instance.AllowView ??= true;

        this.instance.Inputs ??= [];

    },
    mounted() {
        this.refs = [this.$refs['refTestcaseName']];
    },
    watch: {
        testcase() {
            this.instance = this.testcase;
        },
        testcases: {
            handler() {
                if (!this.$cf.isEmptyString(this.instance.TestcaseName)) {
                    this.checkValidate();
                }
            },
            deep: true,
        },
    },
    computed: {
        /**
         * Show button xem status
         */
        isShowStatusButton() {
            if (this.instance && this.instance.Status) {
                return !this.$cf.isEmptyString(this.instance.Status.status_name);
            }
            return false;
        },
        /**
         * Show chi tiết kết quả
         */
        isShowWrongAnswerDetail() {
            if (this.instance && this.instance.Status) {
                return this.instance.Status.status_name == problemEnum.statusJudge0.WrongAnswer;
            }
            return false;
        },
        /**
         * Custom màu button theo status
         */
        severityStatusButton() {
            if (this.instance && this.instance.Status) {
                const statusJudge0 = problemEnum.statusJudge0;
                switch (this.instance.Status.status_name) {
                    case statusJudge0.Accepted:
                        return 'success';
                    case statusJudge0.OverLimit:
                        return 'warning;';
                    default:
                        return 'danger';
                }
            }
            return null;
        },
        logStatus() {
            let log = '';
            if (this.instance && this.instance.Status) {
                let logs = [
                    this.instance.Status.compile_output,
                    this.instance.Status.stderr,
                    this.instance.Status.message,
                ];

                logs = this.$cf.removeNullOrEmpty(logs);


                if (!this.$cf.isEmptyArray(logs)) {
                    logs.forEach(item => item.replace(/\n/g, "<br>"))
                    log = logs.join("<br>");
                }

                return log;

            };
            return log;
        }
    },
    methods: {
        /**
         * Đổi allow view
         */
        clickLock(e) {
            e.stopPropagation();
            this.instance.AllowView = !this.instance.AllowView;
        },
        /**
         * Đóng mở detail
         */
        clickPanel() {
            this.isShowStatus = false;
            this.isShowDetail = !this.isShowDetail;
        },
        /**
         * Đóng mở status detail
         */
        clickStatus(e) {
            e.stopPropagation();
            if (this.instance && this.instance.Status) {
                const statusJudge0 = problemEnum.statusJudge0;
                switch (this.instance.Status.status_id) {
                    case statusJudge0.Accepted:
                        break;
                    case statusJudge0.WrongAnswer:
                        break;
                    case statusJudge0.OverLimit:
                        break;
                    default:
                        break;
                };
                this.isShowDetail = false;
                this.isShowStatus = !this.isShowStatus;
            }
        },
        /**
         * Click xoá
         */
        clickDelete() {
            this.$emit('onDelete', this.testcase);
        },
    }
}
</script>
<style scoped>
.testcase-item {
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    border-radius: 8px;
    transition: background-color 0.2s;
}

.testcase__panel {
    width: 100%;
    height: 60px;
    min-height: 60px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 20px;
    background-color: var(--dark-400);
    cursor: pointer;
    border-radius: 8px;
    transition: background-color 0.2s;
}

.testcase__panel:focus-within {
    background-color: var(--dark-300);
}

.testcase__panel:hover {
    background-color: var(--dark-200);
}

.testcase-item:has(.testcase__detail) .testcase__panel {
    border-radius: 8px 8px 0 0;
}

.testcase__label {
    font-weight: 700;
}

.testcase__function {
    height: 100%;
}

.testcase__detail {
    width: 100%;
    height: fit-content;
    background-color: var(--dark-500);
    border-radius: 0 0 8px 8px;
    padding: 20px;
}

.testcase__detail:has(.btn-hide-detail) {
    padding-bottom: 10px;
}

.testcase__input {
    display: flex;
    flex-direction: column;
    row-gap: 8px;
}</style>