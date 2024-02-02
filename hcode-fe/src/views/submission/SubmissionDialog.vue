<template>
    <v-dialog
        class="v-dialog dark submission-dialog"
        v-model:visible="isVisible"
        style="width: 1200px; height: 800px;"
        modal
        :header="headerSubmission"
        :draggable="false"
        @hide="close"
    >
        <template #closeIcon>
            <v-icon icon="far fa-circle-xmark" />
        </template>
        <div class="v-dialog__content">
            <div class="w-full p-12" style="height: 600px;">
                <v-tab-view>
                    <!-- Lời giải -->
                    <v-tab-panel :header="$t('problem.field.solution')">
                        <div class="w-full flex-column row-gap-20">
                            <v-code-mirror
                                v-model="instance.SourceCode"
                                :language="languageId"
                            ></v-code-mirror>
                        </div>
                    </v-tab-panel>
                    <!-- Testcase -->
                    <v-tab-panel :header="$t('problem.testcases')">
                        <div class="testcase-container dark">
                            <div
                                class="testcase__list"
                                v-if="!$cf.isEmptyArray(instance.Testcases)"
                            >
                                <TestcaseItem
                                    v-for="(testcase, index) in instance.Testcases"
                                    isReadOnly
                                    :key="testcase.TestcaseId"
                                    :index="index"
                                    :testcase="testcase"
                                    :parameters="instance.Parameters"
                                ></TestcaseItem>
                            </div>
                        </div>
                    </v-tab-panel>
                </v-tab-view>
            </div>
        </div>
        <template #footer>
            <v-button-container justifyContent="space-between">
                <!-- Các nút bên trái -->
                <div>
                    <v-button
                        outlined
                        :label="$t('com.close')"
                        @click="close"
                    ></v-button>
                </div>
                <!-- Các nút bên phải -->
                <div class="flex col-gap-8">
                    <!-- Đóng -->
                    <v-button
                        v-if="instance.SourceCode"
                        :label="$t('com.copy')"
                        @click="copySubmission"
                    ></v-button>
                </div>
            </v-button-container>
        </template>
    </v-dialog>
</template>
<script>
import TestcaseItem from "@/views/submit/TestcaseItem.vue";

export default {
    name: "SubmissionDialog",
    components: {
        TestcaseItem,
    },
    props: {
        submission: {
            type: Object,
            default: {}
        },
        languageId: {
            type: Number,
            default: 63,
        },
    },
    data() {
        return {
            instance: {},
            isVisible: false,
        }
    },
    computed: {
        headerSubmission() {
            if (this.instance) {
                let s = this.instance;
                return `${this.$t('problem.field.solution')} - ${s.FullName} - ${s.LanguageName}`
            }
            return ''
        }
    },
    created() {
        this.instance = this.$cf.cloneDeep(this.submission);
        try {
            this.instance.Parameters = JSON.parse(this.instance.Parameters);
            this.instance.Testcases = JSON.parse(this.instance.Testcases);
        } catch (error) {
            console.error(error);
        }
        this.show();
    },
    mounted() {
    },
    methods: {
        show() {
            this.isVisible = true;

        },
        close() {
            this.$emit('close');
        },
        copySubmission() {
            this.$cf.copyToClipboard(this.selectedSubmission.SourceCode);
            this.$ts.success(this.$t("com.copied"));
        }
    }
};
</script>
<style scoped>
/* parameter */
.testcase-container,
.parameter-container {
    display: flex;
    flex-direction: column;
    row-gap: 16px;
    height: fit-content;
    max-height: 100%;
    padding: 4px;
}

.testcase__list,
.parameter__list {
    display: flex;
    flex-direction: column;
    row-gap: 8px;
    height: 100%;
    overflow: auto;
}
</style>
