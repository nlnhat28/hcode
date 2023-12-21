<template>
    <div class="problem-detail-container">
        <div class="problem-detail__header">

        </div>
        <div class="problem-detail__body">
            <v-splitter>
                <v-splitter-panel class="flex-center">
                    <div class="wh-full p-32">
                        <!-- Thông tin chung -->
                        <v-tab-view>
                            <v-tab-panel :header="$t('problem.generalInfo')">
                                <v-form-body class="dark">
                                    <v-form-column :rowGap="32">
                                        <!-- Mã và tên -->
                                        <v-form-group :columnGap="12">
                                            <!-- Mã -->
                                            <v-form-item
                                                style="width: 20%;"
                                                isRequired
                                                :label="$t('problem.field.problemCode')"
                                            >
                                                <v-input-text
                                                    ref="refProblemCode"
                                                    isRequired
                                                    v-model="instance.ProblemCode"
                                                    :maxLength="20"
                                                    :label="$t('problem.field.code')"
                                                    :applyPlaceholder="false"
                                                >
                                                </v-input-text>
                                            </v-form-item>
                                            <!-- Tên -->
                                            <v-form-item
                                                isRequired
                                                :label="$t('problem.field.problemName')"
                                            >
                                                <v-input-text
                                                    ref="refProblemName"
                                                    isRequired
                                                    v-model="instance.ProblemName"
                                                    hasClear
                                                    :maxLength="255"
                                                    :label="$t('problem.field.problemName')"
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
                                                <v-combobox
                                                    v-model="selectedDifficulty"
                                                    optionLabel="name"
                                                    :options="difficulties"
                                                >
                                                </v-combobox>
                                            </v-form-item>
                                            <!-- Chủ đề -->
                                            <v-form-item :label="$t('problem.field.topic')">
                                                <v-input-text
                                                    ref="refTopic"
                                                    v-model="instance.Topic"
                                                    hasClear
                                                    :maxLength="255"
                                                    :label="$t('problem.field.topic')"
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
                                                        hasClear
                                                        :dataInput="$enums.dataInput.decimal"
                                                        :minValue="0"
                                                        :maxValue="10"
                                                        :label="$t('problem.field.limitTime')"
                                                        :applyPlaceholder="false"
                                                    >
                                                    </v-input-text>
                                                    <!-- <v-combobox
                                                        style="width: 60%;"
                                                        v-model="selectedTimeUnit"
                                                        optionLabel="name"
                                                        :options="$cv.enumToSelects($enums.timeUnit)"
                                                    >
                                                    </v-combobox> -->
                                                </div>
                                            </v-form-item>
                                            <!-- Bộ nhớ -->
                                            <v-form-item :label="$t('problem.field.limitMemory')">
                                                <div class="flex-align-center col-gap-8">
                                                    <v-input-text
                                                        v-model="instance.LimitMemory"
                                                        type='number'
                                                        hasClear
                                                        :dataInput="$enums.dataInput.decimal"
                                                        :minValue="0"
                                                        :maxValue="51200"
                                                        :label="$t('problem.field.limitMemory')"
                                                        :applyPlaceholder="false"
                                                    >
                                                    </v-input-text>
                                                    <!-- <v-combobox
                                                        style="width: 60%;"
                                                        v-model="selectedMemoryUnit"
                                                        optionLabel="name"
                                                        :options="$cv.enumToSelects($enums.memoryUnit)"
                                                    ></v-combobox> -->
                                                </div>
                                            </v-form-item>
                                            <!-- Trạng thái lưu -->
                                            <!-- <v-form-item :label="$t('problem.field.limitMemory')">
                                                <div class="flex-align-center col-gap-8">
                                                    <v-input-text
                                                        v-model="instance.LimitMemory"
                                                        :dataInput="$enums.dataInput.integer"
                                                    >
                                                    </v-input-text>
                                                    <v-combobox
                                                        style="width: 60%;"
                                                        v-model="selectedMemoryUnit"
                                                        optionLabel="name"
                                                        :options="$cv.enumToSelects($enums.memoryUnit)"
                                                    ></v-combobox>
                                                </div>
                                            </v-form-item> -->
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
                                            <v-combobox
                                                v-model="selectedOutputType"
                                                optionLabel="name"
                                                :options="dataTypes"
                                            >
                                            </v-combobox>
                                        </v-form-item>
                                        <!-- Tham số đầu vào-->
                                        <v-form-item :label="$t('problem.inputParameter')">
                                            <v-button-container class="w-full justify-between">
                                                <v-button
                                                    icon="far fa-plus"
                                                    :label="$t('com.add')"
                                                    @click="clickAddParameter"
                                                ></v-button>
                                                <v-button
                                                    v-if="!$cf.isEmptyArray(instance.Parameters)"
                                                    severity="danger"
                                                    icon="far fa-trash-can"
                                                    outlined
                                                    :label="$t('com.deleteAll')"
                                                    @click="clickDeleteAllParameter"
                                                ></v-button>
                                            </v-button-container>
                                            <div
                                                class="parameter__list"
                                                v-if="!$cf.isEmptyArray(instance.Parameters)"
                                            >
                                                <ParameterItem
                                                    v-for="(param, index) in instance.Parameters"
                                                    :key="param.ParameterId"
                                                    :index="index"
                                                    :parameter="param"
                                                    :parameters="instance.Parameters"
                                                    @onDelete="deleteParameter"
                                                ></ParameterItem>
                                            </div>
                                        </v-form-item>
                                    </v-form-column>
                                </div>
                            </v-tab-panel>
                            <!-- Test -->
                            <v-tab-panel :header="$t('problem.testcase')">
                                <div class="testcase-container dark">
                                    <v-button-container class="w-full justify-between">
                                        <v-button
                                            icon="far fa-plus"
                                            :label="$t('com.add')"
                                            @click="clickAddTestcase"
                                        ></v-button>
                                        <v-button
                                            v-if="!$cf.isEmptyArray(instance.Testcases)"
                                            severity="danger"
                                            icon="far fa-trash-can"
                                            outlined
                                            :label="$t('com.deleteAll')"
                                            @click="clickDeleteAllTestcase"
                                        ></v-button>
                                    </v-button-container>
                                    <div
                                        class="testcase__list"
                                        v-if="!$cf.isEmptyArray(instance.Testcases)"
                                    >
                                        <TestcaseItem
                                            v-for="(testcase, index) in instance.Testcases"
                                            :key="testcase.TestcaseId"
                                            :index="index"
                                            :testcase="testcase"
                                            :testcases="instance.Testcases"
                                            :parameters="instance.Parameters"
                                            @onDelete="deleteTestcase"
                                        ></TestcaseItem>
                                    </div>
                                </div>
                            </v-tab-panel>
                        </v-tab-view>
                    </div>
                </v-splitter-panel>
                <!-- Code Editor -->
                <v-splitter-panel class="flex-center">
                    <div class="wh-full p-20 flex-column code-container">
                        <div class="code__header">
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
                            <v-code-editor
                                class="v-code-editor"
                                v-model="instance.SourceCode"
                            ></v-code-editor>
                        </div>
                        <div class="code__footer">

                        </div>
                    </div>
                </v-splitter-panel>
            </v-splitter>
        </div>
        <div class="problem-detail__footer">
            <v-button-container direction="row-reverse">
                <!-- Lưu -->
                <v-button
                    :label="$t('com.save')"
                    @click="onClickSave"
                />
                <!-- Lưu nháp -->
                <v-button
                    :label="$t('com.saveDraft')"
                    outlined
                    @click="onClickSaveDraft"
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
import enums from "@/enums/enums";
const formMode = enums.formMode;

export default {
    name: "ProblemDetail",
    extends: BaseForm,
    components: {
        ParameterItem,
        TestcaseItem,
    },
    data() {
        return {
            instanceService: problemService,
            problemConst: problemConst,
            languages: [],
            difficulties: [],
            dataTypes: [],
            sourceCodes: [],
            // selectedTimeUnit: null,
            // selectedMemoryUnit: null,
            selectedDifficulty: null,
            selectedOutputType: null,
        }
    },
    watch: {
        // selectedTimeUnit: {
        //     handler() {
        //         this.instance.TimeUnit = this.$cv.selectedToEnumKey(this.selectedTimeUnit);
        //     },
        //     deep: true
        // },
        // selectedMemoryUnit: {
        //     handler() {
        //         this.instance.MemoryUnit = this.$cv.selectedToEnumKey(this.selectedMemoryUnit);
        //     },
        //     deep: true
        // },
        selectedDifficulty: {
            handler() {
                this.instance.Difficulty = this.$cv.selectedToEnumKey(this.selectedDifficulty);
            },
            deep: true
        },
        selectedOutputType: {
            handler() {
                this.instance.OutputType = this.$cv.selectedToEnumKey(this.selectedOutputType);
            },
            deep: true
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
    },
    methods: {
        /**
         * Khởi tạo dữ liệu data
         */
        async initOnCreated() {

            let id = this.$route.params.id;

            if (id == null) {
                this.mode = formMode.create;
            }

            this.instance = this.problemStore.problem;

            this.instance.SourceCode ??= '';

            this.difficulties = this.$cv.enumToSelects(enums.difficulty);
            this.selectedDifficulty = this.$cv.enumKeyToSelected(this.instance.Difficulty, this.difficulties, 0);

            this.dataTypes = this.$cv.enumToSelects(enums.dataType);
            this.selectedOutputType = this.$cv.enumKeyToSelected(this.instance.OutputType, this.dataTypes, 0);
            // this.selectedTimeUnit = this.$cv.enumToSelects(enums.timeUnit)[0];
            // this.selectedMemoryUnit = this.$cv.enumToSelects(enums.memoryUnit)[0];
        },
        /**
         * Lấy dữ liệu
         */
        async loadDataOnCreated() {
            await this.getLanguages();
            if (this.$cf.isEmptyObject(this.instance.SolutionLanguage) && !this.$cf.isEmptyArray(this.languages)) {
                this.instance.SolutionLanguage = this.languages[0];
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
        buildSourceCode() {

        },
        defauftSourceCode() {
            let sourceCodes = [];
            
            if (this.languages) {
                const lang = enums.language;
                for (const language of this.languages) {
                    switch (language.JudgeId) {
                        case lang.c:

                            break;

                        default:
                            break;
                    }
                }
            }
        },
        /**
         * Thêm param
         */
        clickAddParameter() {
            if (!this.instance.Parameters) {
                this.instance.Parameters = []
            };

            const param = {
                ParameterId: this.$cf.uuid.new(),
            }

            this.instance.Parameters.push(param);
        },
        /**
         * Xoá 1 parameter
         * 
         * @param {*} parameter 
         */
        deleteParameter(parameter) {
            this.instance.Parameters = this.instance.Parameters.filter(item => item.ParameterId != parameter.ParameterId)
        },
        /**
         * Click xoá tham số
         */
        clickDeleteAllParameter() {
            const header = this.$t("problem.deleteAllParam");
            const message = this.$t('problem.deleteAllParamConfirm');
            const buttons = [
                {
                    // Huỷ
                    severity: "secondary",
                    outlined: true,
                    label: this.$t("com.cancel"),
                    icon: "fa fa-xmark",
                },
                {
                    // Gửi
                    severity: "danger",
                    label: this.$t("com.delete"),
                    icon: "far fa-trash-can",
                    autofocus: true,
                    click: this.deleteAllParameter,
                }
            ];
            this.$dl.confirm(message, buttons, header);
        },
        /**
         * Xoá hết tham số
         */
        deleteAllParameter() {
            this.instance.Parameters = [];
        },
        /**
         * Thêm Testcase
         */
        clickAddTestcase() {
            if (!this.instance.Testcases) {
                this.instance.Testcases = []
            };

            const testcase = {
                TestcaseId: this.$cf.uuid.new(),
            }

            this.instance.Testcases.push(testcase);
        },
        /**
         * Xoá 1 testcase
         * 
         * @param {*} testcase 
         */
        deleteTestcase(testcase) {
            this.instance.Testcases = this.instance.Testcases.filter(item => item.TestcaseId != testcase.TestcaseId)
        },
        /**
         * Click xoá hết testcase
         */
        clickDeleteAllTestcase() {
            const header = this.$t("problem.deleteAllTestcase");
            const message = this.$t('problem.deleteAllTestcaseConfirm');
            const buttons = [
                {
                    // Huỷ
                    severity: "secondary",
                    outlined: true,
                    label: this.$t("com.cancel"),
                    icon: "fa fa-xmark",
                },
                {
                    // Gửi
                    severity: "danger",
                    label: this.$t("com.delete"),
                    icon: "far fa-trash-can",
                    autofocus: true,
                    click: this.deleteAllTestcase,
                }
            ];
            this.$dl.confirm(message, buttons, header);
        },
        /**
         * Xoá hết testcase
         */
        deleteAllTestcase() {
            this.instance.Testcases = [];
        },
        /**
         * Reformat instance trước khi lưu
         *
         * Author: nlnhat (02/07/2023)
         */
        reformatInstance() {
            return this.instance;
        },
        /**
         * Click lưu
         */
        customBeforeSave() {
            this.instance.State = problemEnum.problemState.private.value;
        },
        /**
         * Click lưu nháp
         */
        onClickSaveDraft() {
            this.instance.State = problemEnum.problemState.draft.value;
            console.log(this.instance);
        }
    }
}
</script>
<style scoped>
@import './problem-detail.css';
</style>