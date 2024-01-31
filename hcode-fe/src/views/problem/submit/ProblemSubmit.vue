<template>
    <div class="problem-submit-container">
        <!-- Loading -->
        <v-mask-loading v-if="isLoading" />
        <div class="problem-submit__header">
            <div class="problem-submit__header--left flex-align-center">
                <v-button
                    text
                    severity="secondary"
                    icon="far fa-angle-left"
                    :label="$t('problem.problemList')"
                    @click="$router.push($path.problems)"
                ></v-button>
            </div>
            <div class="problem-submit__header--center flex-center col-gap-12">
                <div class="label-20-yellow-300">
                    {{ centerTitle }}
                </div>
            </div>
            <div class="problem-submit__header--right">

            </div>
        </div>
        <div class="problem-submit__body">
            <v-splitter>
                <v-splitter-panel class="flex-center">
                    <div class="wh-full p-32">
                        <v-tab-view :activeIndex="activeTab">
                            <!-- Thông tin chung -->
                            <v-tab-panel :header="$t('problem.generalInfo')">
                                <v-form-body class="dark">
                                    <v-form-column :rowGap="32">
                                        <!-- Mã và tên -->
                                        <v-form-group :columnGap="12">
                                            <!-- Tên -->
                                            <v-form-item
                                                isRequired
                                                :label="$t('problem.field.problemName')"
                                            >
                                                <v-input-text
                                                    ref="refProblemName"
                                                    v-model="instance.ProblemName"
                                                    isReadOnly
                                                    :applyPlaceholder="false"
                                                >
                                                </v-input-text>
                                            </v-form-item>
                                        </v-form-group>
                                        <!-- Độ khó và chủ đề -->
                                        <v-form-group :columnGap="12">
                                            <!-- Độ khó -->
                                            <v-form-item
                                                style="width: 20%;"
                                                :label="$t('problem.field.difficulty')"
                                            >
                                                <v-input-text
                                                    ref="refDifficulty"
                                                    isReadOnly
                                                    :value="selectedDifficulty?.name"
                                                    :applyPlaceholder="false"
                                                >
                                                </v-input-text>
                                            </v-form-item>
                                            <!-- Chủ đề -->
                                            <v-form-item :label="$t('problem.field.topic')">
                                                <v-input-text
                                                    ref="refTopic"
                                                    v-model="instance.Topic"
                                                    isReadOnly
                                                    :applyPlaceholder="false"
                                                >
                                                </v-input-text>
                                            </v-form-item>
                                        </v-form-group>
                                        <!-- Giới hạn -->
                                        <v-form-group :columnGap="12">
                                            <!-- Thời gian -->
                                            <v-form-item :label="$t('problem.field.limitTime')">
                                                <div class="flex-align-center col-gap-8">
                                                    <v-input-text
                                                        v-model="instance.LimitTime"
                                                        type='number'
                                                        isReadOnly
                                                        :dataInput="$enums.dataInput.decimal"
                                                        :applyPlaceholder="false"
                                                    >
                                                    </v-input-text>
                                                </div>
                                            </v-form-item>
                                            <!-- Bộ nhớ -->
                                            <v-form-item :label="$t('problem.field.limitMemory')">
                                                <div class="flex-align-center col-gap-8">
                                                    <v-input-text
                                                        v-model="instance.LimitMemory"
                                                        type='number'
                                                        isReadOnly
                                                        :dataInput="$enums.dataInput.decimal"
                                                        :applyPlaceholder="false"
                                                    >
                                                    </v-input-text>
                                                </div>
                                            </v-form-item>
                                        </v-form-group>
                                    </v-form-column>
                                </v-form-body>
                            </v-tab-panel>
                            <!-- Nội dung -->
                            <v-tab-panel :header="$t('problem.content')">
                                <v-editor v-model="instance.Content"></v-editor>
                            </v-tab-panel>
                            <!-- Gợi ý -->
                            <v-tab-panel :header="$t('problem.hint')">
                                <v-editor v-model="instance.Hint"></v-editor>
                            </v-tab-panel>
                            <!-- Tham số-->
                            <v-tab-panel :header="$t('problem.parameter')">
                                <div class="parameter-container dark">
                                    <v-form-column :rowGap="24">
                                        <!-- Kiểu dữ liệu trả về-->
                                        <v-form-item
                                            style="width: 20%;"
                                            :label="$t('problem.outputType')"
                                        >
                                            <v-input-text
                                                ref="refOutputType"
                                                isReadOnly
                                                :value="selectedOutputType?.name"
                                                :applyPlaceholder="false"
                                            >
                                            </v-input-text>
                                        </v-form-item>
                                        <!-- Tham số đầu vào-->
                                        <v-form-item :label="$t('problem.inputParameter')">
                                            <div
                                                class="parameter__list"
                                                v-if="!$cf.isEmptyArray(instance.Parameters)"
                                            >
                                                <ParameterItem
                                                    v-for="(param, index) in instance.Parameters"
                                                    isReadOnly
                                                    :key="param.ParameterId"
                                                    :index="index"
                                                    :parameter="param"
                                                ></ParameterItem>
                                            </div>
                                        </v-form-item>
                                    </v-form-column>
                                </div>
                            </v-tab-panel>
                            <!-- Test -->
                            <v-tab-panel :header="$t('problem.testcase')">
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
                            <!-- Đã nộp -->
                            <v-tab-panel :header="$t('problem.submissions')">
                                <SubmissionsList
                                    ref="refSubmissionList"
                                    :parentId="instance.ProblemAccountId"
                                    v-if="instance.ProblemAccountId"
                                    @selected="bindSubmission"
                                />
                            </v-tab-panel>
                        </v-tab-view>
                    </div>
                </v-splitter-panel>
                <!-- Code Editor -->
                <v-splitter-panel class="flex-center">
                    <div class="wh-full p-20 flex-column code-container">
                        <div class="code__header flex justify-between">
                            <div class="flex-align-center col-gap-12">
                                <div class="font-bold color-text flex-center">
                                    {{ $t('problem.field.solution') }}
                                </div>
                                <v-button
                                    icon="far fa-arrows-rotate"
                                    severity="danger"
                                    text
                                    raised
                                    rounded
                                    :title="'Reset'"
                                    @click="clickResetSource"
                                />
                            </div>
                            <div class="flex-align-center w-fit">
                                <v-combobox
                                    class="transparent no-border"
                                    v-model="instance.SolutionLanguage"
                                    optionLabel="LanguageName"
                                    :options="languages"
                                ></v-combobox>
                            </div>
                        </div>
                        <div class="code__body">
                            <v-code-mirror
                                v-model="instance.Solution"
                                :language="instance.SolutionLanguage"
                            ></v-code-mirror>
                        </div>
                        <div class="code__footer">

                        </div>
                    </div>
                </v-splitter-panel>
            </v-splitter>
        </div>
        <div class="problem-submit__footer">
            <v-button-container direction="row-reverse">
                <!-- Nộp -->
                <v-button
                    :label="$t('problem.submitSubmission')"
                    @click="clickSubmit"
                />
            </v-button-container>
        </div>
    </div>
</template>
<script>
import BaseProblemSubmit from "@/views/submit/BaseProblemSubmit.vue";
import problemEnum from "@/enums/problem-enum";
import SubmissionsList from "./submission/SubmissionsList.vue";

export default {
    name: "ProblemSubmit",
    extends: BaseProblemSubmit,
    components: {
        SubmissionsList
    },
    data() {
        return {
            cfg: {
                formPath: this.$path.problem,
                callbackPath: this.$path.problems,
                entity: 'Problem',
                subSysName: this.$t('problem.problem'),
            },
        }
    },
    methods: {
        /** 
         * @override
         */
        customInstanceOnCreated() {
            this.selectedOutputType = this.$cv.enumKeyToSelected(this.instance.OutputType, this.dataTypes, 0);
            this.selectedDifficulty = this.$cv.enumKeyToSelected(this.instance.Difficulty, this.difficulties, 0);

            if (!this.$cf.isEmptyArray(this.languages)) {
                this.instance.SolutionLanguage = this.languages[0];
            }
            this.auditProblemAccount();
        },
        /**
         * Tạo quan hệ bài toán tài khoản
         */
        auditProblemAccount() {
            if (!this.instance.ProblemAccountState && this.instance.ProblemId) {
                let payload = {
                    ProblemId: this.instance.ProblemId,
                    State: problemEnum.problemAccountState.seen.value,
                };

                this.instanceService.auditProblemAccount(payload);
            }
        },
    }
}
</script>
<style scoped>
@import './problem-submit.css';
</style>