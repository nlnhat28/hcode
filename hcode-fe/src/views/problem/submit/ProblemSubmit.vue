<template>
    <div class="problem-detail-container">
        <!-- Loading -->
        <v-mask-loading v-if="isLoading" />
        <div class="problem-detail__header">
            <div class="problem-detail__header--left flex-align-center">
                <v-button
                    text
                    severity="secondary"
                    icon="far fa-angle-left"
                    :label="$t('problem.problemList')"
                    @click="$router.push($path.problems)"
                ></v-button>
            </div>
            <div class="problem-detail__header--center flex-center col-gap-12">
                <div class="font-bold font-20 yellow-300">
                    {{ centerTitle }}
                </div>
            </div>
            <div class="problem-detail__header--right">

            </div>
        </div>
        <div class="problem-detail__body">
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
                                        <div class="flex col-gap-24">
                                            <div class="checkbox-item">
                                                <!-- Trạng thái lưu -->
                                                <div class="label cursor-pointer">
                                                    {{ $t('problem.problemState.public') }}
                                                </div>
                                                <v-checkbox
                                                    v-model="instance.IsPublicState"
                                                    binary
                                                    :disabled="mode == $enums.formMode.update"
                                                >
                                                </v-checkbox>
                                            </div>
                                            <div class="checkbox-item">
                                                <!-- Trạng thái lưu -->
                                                <div class="label cursor-pointer">
                                                    {{ $t('problem.problemState.private') }}
                                                </div>
                                                <v-checkbox
                                                    v-model="instance.IsPrivateState"
                                                    binary
                                                    :disabled="mode == $enums.formMode.update"
                                                >
                                                </v-checkbox>
                                            </div>
                                        </div>
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
                            <!-- Thông báo kết quả chạy -->
                            <!-- <v-tab-panel :header="$t('problem.result')">
                                <v-editor
                                    class='no-toolbar'
                                    v-model="result"
                                    readonly
                                ></v-editor>
                            </v-tab-panel> -->
                            <!-- Đã nộp -->
                            <!-- <v-tab-panel :header="$t('problem.submissions')">
                                <SubmissionsList :parentId="instance.ProblemAccountId" />
                            </v-tab-panel> -->
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
        <div class="problem-detail__footer">
            <v-button-container
                direction="row-reverse"
                v-if="showExit"
            >
                <!-- Đóng -->
                <v-button
                    v-if="showExit"
                    :label="$t('com.close')"
                    @click="$router.push($path.problems)"
                />
            </v-button-container>
            <v-button-container
                direction="row-reverse"
                v-else
            >
                <!-- Lưu -->
                <v-button
                    :label="$t('com.submit')"
                    @click="clickSubmit"
                />
            </v-button-container>
        </div>
    </div>
</template>
<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { problemService, languageService } from "@/services/services";
import { useLanguageStore, useProblemStore } from "@/stores/stores";
import { mapStores, mapState } from 'pinia';
import problemEnum from "@/enums/problem-enum";
import problemConst from "@/consts/problem-const.js";
import ParameterItem from "./ParameterItem.vue";
import TestcaseItem from "./TestcaseItem.vue";
import SubmissionsList from "./submission/SubmissionsList.vue";
import enums from "@/enums/enums";

const formMode = enums.formMode;

export default {
    name: "ProblemSubmit",
    extends: BaseForm,
    components: {
        ParameterItem,
        TestcaseItem,
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
            instanceService: problemService,
            problemConst: problemConst,
            languages: [],
            difficulties: [],
            dataTypes: [],
            sourceCodes: [],
            selectedDifficulty: null,
            selectedOutputType: null,
            tabView: {
                info: 0,
                parameter: 3,
                test: 4,
            },
            activeTab: 0,
            result: null,
            allowBuildSource: false,
            isDraft: false,
            showExit: false,
        }
    },
    watch: {
        // Gán độ khó
        selectedDifficulty: {
            handler() {
                this.instance.Difficulty = this.$cv.selectedToEnumKey(this.selectedDifficulty);
            },
            deep: true
        },
        // Gán kiểu trả về
        selectedOutputType: {
            handler() {
                this.instance.OutputType = this.$cv.selectedToEnumKey(this.selectedOutputType);
            },
            deep: true
        },
        // Build source code khi những thứ này thay đổi
        "instance.SolutionLanguage": {
            handler() {
                if (this.checkBuildSource) {
                    this.buildSourceCode();
                }
            },
            deep: true
        },
        "instance.Parameters": {
            handler() {
                if (this.checkBuildSource) {
                    this.buildSourceCode();
                }
            },
            deep: true
        },
        "instance.OutputType": {
            handler() {
                if (this.checkBuildSource) {
                    this.buildSourceCode();
                }
            },
            deep: true
        },
        "instance.IsPublicState"(newVal, oldVal) {
            if (!this.instance.IsPrivateState && !newVal) {
                this.instance.IsPublicState = oldVal;
            }
        },
        "instance.IsPrivateState"(newVal, oldVal) {
            if (!this.instance.IsPublicState && !newVal) {
                this.instance.IsPrivateState = oldVal;
            }
        }
    },
    mounted() {
        this.refs = [
            this.$refs.refProblemName,
            this.$refs.refProblemCode,
        ]
    },
    computed: {
        ...mapStores(useLanguageStore),
        ...mapStores(useProblemStore),
        /**
         * Source code mặc định theo kiểu dữ liệu trả về, parameter, ngôn ngữ
         */
        defaultSourceCode() {
            let funcName = problemConst.solutionFunction;
            let className = problemConst.solutionClass;

            const outputType = this.instance.OutputType;
            const params = this.instance.Parameters;
            const lang = problemEnum.language;

            if (this.languages) {
                // for (const language of this.languages) {

                let paramItems = [];
                let outputCode = '';
                let paramCode = '';
                let sourceCode = '';
                let tab = '    ';
                let tab2 = tab + tab;

                // const judgeId = language.JudgeId;
                const judgeId = this.instance.SolutionLanguage?.JudgeId;

                // Build output
                outputCode = this.dataTypeByLanguage(outputType, judgeId) ?? '';

                // Build parameter
                if (params) {
                    for (const param of params) {

                        if (!this.$cf.isEmptyString(param.ParameterName)) {

                            let paramItem = this.dataTypeByLanguage(param.DataType, judgeId);

                            if (!this.$cf.isEmptyString(paramItem)) {
                                paramItem += ' ';
                            };

                            if (judgeId == problemEnum.language.php) {
                                paramItem += '$'
                            }

                            paramItem += param.ParameterName;

                            paramItems.push(paramItem);
                        }
                    };

                    paramItems = this.$cf.removeNullOrEmpty(paramItems);

                    paramCode = paramItems.join(', ');
                }

                switch (judgeId) {
                    case lang.c:
                        sourceCode = `${outputCode} ${funcName}(${paramCode}) {\n${tab}\n}`;
                        break;
                    case lang.cpp:
                        sourceCode = `class ${className} {\npublic:\n${tab}${outputCode} ${funcName}(${paramCode}) {\n${tab2}\n${tab}}\n}`;
                        break;
                    case lang.csharp:
                    case lang.java:
                        sourceCode = `class ${className} {\n${tab}public ${outputCode} ${funcName}(${paramCode}) {\n${tab2}\n${tab}}\n}`;
                        break;
                    case lang.js:
                        sourceCode = `const ${funcName} = function(${paramCode}) {\n    \n}`;
                        break;
                    case lang.php:
                        sourceCode = `class ${className} {\n${tab}function ${funcName}(${paramCode}) {\n${tab2}\n${tab}}\n}`;
                        break;
                    case lang.python:
                        sourceCode = `class ${className}():\n${tab}def ${funcName}(self, ${paramCode}) {\n${tab2}\n}`;
                        break;
                    default:
                        break;
                }
                // }

                return sourceCode;
            }
            return ''
        },
        checkBuildSource() {
            return this.mode != this.$enums.formMode.update || this.allowBuildSource || this.$cf.isEmptyString(this.instance.Solution)
        },
        /**
         * Tạo tiêu đề problem
         */
        centerTitle() {
            let title = '';
            if (!this.$cf.isEmptyString(this.instance.ProblemCode)) {
                title = `${this.instance.ProblemCode}. `;
            };
            if (!this.$cf.isEmptyString(this.instance.ProblemName)) {
                title += this.instance.ProblemName;
            }

            return title;
        },
    },
    methods: {
        /**
         * Khởi tạo dữ liệu data
         */
        async initOnCreated() {
            this.difficulties = this.$cv.enumToSelects(enums.difficulty);
            this.dataTypes = this.$cv.enumToSelects(problemEnum.dataType);
        },
        /**
         * Khởi tạo problem khi thêm mới
         * @override
         */
        initCreateInstance() {
            this.documentTitle = this.$t("problem.createProblem");
            document.title = this.$cf.documentTitle(this.documentTitle);

            this.instance.Solution ??= '';
            this.instance.IsPrivateState = true;
            this.instance.TimeUnit = this.$enums.timeUnit.second.value;
            this.instance.MemoryUnit = this.$enums.memoryUnit.kilobyte.value;
        },
        /** 
         * @overrid 
         * 
         */
        customInstanceOnCreated() {
            this.selectedDifficulty = this.$cv.enumKeyToSelected(this.instance.Difficulty, this.difficulties, 0);
            this.selectedOutputType = this.$cv.enumKeyToSelected(this.instance.OutputType, this.dataTypes, 0);
            this.selectedDifficulty = this.$cv.enumKeyToSelected(this.instance.Difficulty, this.difficulties, 0);

            this.instance.IsPrivateState = this.instance.State == problemEnum.problemState.private.value;
            this.instance.IsPublicState = this.instance.State == problemEnum.problemState.public.value;

            this.isDraft = this.instance.IsDraft;
        },
        /**
         * Lấy dữ liệu
         */
        async loadDataOnCreated() {
            await this.getLanguages();
            if (this.$cf.isEmptyObject(this.instance.SolutionLanguage) && !this.$cf.isEmptyArray(this.languages)) {
                this.instance.SolutionLanguage ??= this.languages[0];
            }
        },
        /**
         * Lấy danh sách language
         */
        async getLanguages() {
            if (this.$cf.isEmptyArray(this.languageStore.languages)) {
                try {
                    const res = await languageService.getAll();
                    if (this.$cf.isSuccess(res)) {
                        this.languages = this.$cf.cloneDeep(res.Data);
                        this.languageStore.setLanguages(res.Data);
                    }
                }
                catch {
                }
            }
            else {
                this.languages = this.$cf.cloneDeep(this.languageStore.languages);
            }
        },
        /**
         * Build source code theo ngôn ngữ
         */
        buildSourceCode() {
            this.instance.Solution = this.defaultSourceCode;
        },
        /**
         * Click reset lời giải
         */
        clickResetSource() {
            this.buildSourceCode();
            this.allowBuildSource = true;
        },
        /**
         * Lấy kiểu dữ liệu theo language
         * 
         * @param {*} dataType DataType enum
         * @param {*} langId JugdeId
         */
        dataTypeByLanguage(dataType, judgeId) {
            const value = problemEnum.mapDataTypeByLanguage(dataType, judgeId);
            return value;
        },
        /**
         * Trước khi doSave() trong clickSave()
         * @virtual
         */
        beforeDoSave() {
            this.instance.IsDraft = false;
            this.instance.State = problemEnum.problemState.private.value;
            this.resetTestcaseStatus();
        },
        /**
         * Click lưu nháp
         */
        onClickSaveDraft() {
            try {
                this.instance.IsDraft = true;
                this.instance.State = problemEnum.problemState.private.value;
                this.doSave();
            } catch (error) {
                console.error(error);
            }
            // this.onClickSave();
        },
        /**
         * Xử lý thêm validate tại đây
         * 
         * Author: nlnhat1 (01/11/2023)
         */
        customValidate() {
            // Validate testcase
            if (this.messageValidate == null) {

                if (this.$cf.isEmptyArray(this.instance.Testcases)) {
                    this.messageValidate = this.$t("problem.mustHasTestcase");
                    this.focusTabView(this.tabView.test);
                }
            };
        },
        /**
         * Focus vào tab view
         * 
         * @param {*} tabViewIndex Index tab
         */
        focusTabView(tabViewIndex) {
            this.activeTab = tabViewIndex;
        },
        /**
         * Reset kết quả các testcases
         */
        resetTestcaseStatus() {
            if (this.instance.Testcases) {
                for (let test of this.instance.Testcases) {
                    test.Status = {};
                }
            }
        },
        /**
         * Map submission vào testcases
         */
        processSubmissionResponse(response) {
            if (response && response.Data) {
                let submissions = this.$cf.cloneDeep(response.Data.Submissions);
                if (!this.$cf.isEmptyArray(submissions) && !this.$cf.isEmptyArray(this.instance.Testcases)) {
                    for (let testcase of this.instance.Testcases) {
                        let sub = submissions.find(item => item.testcase_id == testcase.TestcaseId)
                        if (sub) {
                            testcase.Status = { ...sub };
                        }
                    }
                }
            }
        },
        /**
         * Xử lý response createInstance
         * @override
         */
        processResponseCreate(response) {
            this.processSubmissionResponse(response);
        },
        /**
         * Xử lý response updateInstance
         * @override
         */
        processResponseUpdate(response) {
            this.processSubmissionResponse(response);
        },
        /**
         * Lưu thành công
         */
        afterSaveSuccess() {
            this.$ts.success(this.messageOnToast);
            this.showExit = true;
        },
        /**
         * Hiển thị submission response lên tab kết quả
         */
        showSubmissionResponse(statusName, content, isSuccess) {
            let title = statusName;
            let body = content;
            let divide = '<p><hr></p>';
            let color = isSuccess == true ? '#00ff00' : '#ff0000';

            title = `<p><strong style=\"color: ${color};\">${title}</strong></p>`;
            body = `<p><span style=\"color: rgb(187, 187, 187);\">${body}</span></p>`;

            let fullContent = title + divide + body;

            this.result.log = fullContent;
        },
    }
}
</script>
<style scoped>
@import './problem-detail.css';
</style>