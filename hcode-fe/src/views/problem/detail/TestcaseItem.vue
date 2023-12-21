<template>
    <div class="testcase-item">
        <div
            class="testcase__panel"
            @click="clickPanel"
        >
            <div class="testcase__label flex-center">
                {{ index + 1 }}. Testcase {{ index + 1 }}
            </div>
            <div class="testcase__function flex-align-center col-gap-12">
                <v-icon
                    applyPointer
                    :icon="this.instance.AllowView ? 'far fa-lock-keyhole-open' : 'far fa-lock-keyhole'"
                    :color="this.instance.AllowView ? 'light' : 'warn'"
                    @click="clickLock"
                />
                <v-icon
                    icon="far fa-circle-xmark"
                    color="danger"
                    applyPointer
                    @click="clickDelete"
                />
            </div>
        </div>
        <div
            class="testcase__detail flex-column row-gap-20"
            v-if="isShowDetail"
        >
            <div class="flex-column row-gap-12">
                <div
                    class="testcase__input"
                    v-for="(param, index) in parameters"
                    :key="index"
                >
                    <div class="testcase__parameter-name">
                        {{ param.ParameterName || `${$t('problem.parameter')} ${param.ParameterOrder}` }}
                    </div>
                    <div class="testcase__parameter-input">
                        <v-input-text hasClear v-model="instance.Inputs[index]"></v-input-text>
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
        </div>
    </div>
</template>
<script>
import { validate } from "@/mixins/mixins";

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
            isShowDetail: false,
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
            this.isShowDetail = !this.isShowDetail;
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

.testcase__input {
    display: flex;
    flex-direction: column;
    row-gap: 8px;
}</style>
