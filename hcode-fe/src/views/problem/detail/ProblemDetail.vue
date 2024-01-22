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
                <v-tag
                    v-if="this.isDraft"
                    class="p-tag-draft"
                    :value="$t('problem.draft')"
                />
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
                                            <!-- Mã -->
                                            <!-- <v-form-item
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
                                            </v-form-item> -->
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
                                                        :dataInput="$enums.dataInput.decimal"
                                                        :minValue="0"
                                                        :maxValue="10"
                                                        :label="$t('problem.field.limitTime')"
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
                                                        :dataInput="$enums.dataInput.decimal"
                                                        :minValue="2048"
                                                        :maxValue="51200"
                                                        :label="$t('problem.field.limitMemory')"
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
                    :label="$t('com.save')"
                    @click="onClickSave"
                />
                <!-- Lưu nháp -->
                <v-button
                    outlined
                    :label="$t('com.saveDraft')"
                    @click="onClickSaveDraft"
                />
            </v-button-container>
        </div>
    </div>
</template>
<script>
import BaseProblemDetail from "@/views/problem/base/BaseProblemDetail.vue";
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
    extends: BaseProblemDetail,
    components: {
        ParameterItem,
        TestcaseItem,
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
        }
    },
    mounted() {
        this.refs = [
            this.$refs.refProblemName,
            this.$refs.refProblemCode,
        ]
    },
    computed: {
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
         * @override
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