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
                                                <v-input-text>
                                                </v-input-text>
                                            </v-form-item>
                                            <!-- Tên -->
                                            <v-form-item
                                                isRequired
                                                :label="$t('problem.field.problemName')"
                                            >
                                                <v-input-text
                                                    hasClear
                                                    :label="$t('problem.field.problemName')"
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
                                                    v-model="instance.difficulty"
                                                    optionLabel="name"
                                                    :options="$cv.enumToSelects($enums.difficulty)"
                                                >
                                                </v-combobox>
                                            </v-form-item>
                                            <!-- Chủ đề -->
                                            <v-form-item :label="$t('problem.field.topic')">
                                                <v-input-text>
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
                                                        :dataInput="$enums.dataInput.integer"
                                                    >
                                                    </v-input-text>
                                                    <v-combobox
                                                        style="width: 60%;"
                                                        v-model="selectedTimeUnit"
                                                        optionLabel="name"
                                                        :options="$cv.enumToSelects($enums.timeUnit)"
                                                    >
                                                    </v-combobox>
                                                </div>
                                            </v-form-item>
                                            <!-- Bộ nhớ -->
                                            <v-form-item :label="$t('problem.field.limitMemory')">
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
                                </div>
                            </v-tab-panel>
                            <!-- Test -->
                            <v-tab-panel :header="$t('problem.testcase')">
                                <div class="parameter-container dark">
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
                                        <TestcaseItem
                                            v-for="(param, index) in instance.Parameters"
                                            :key="param.ParameterId"
                                            :index="index"
                                            :parameter="param"
                                            :parameters="instance.Parameters"
                                            @onDelete="deleteParameter"
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
                                    v-model="selectedLanguage"
                                    optionLabel="LanguageName"
                                    :options="languages"
                                ></v-combobox>
                            </div>
                        </div>
                        <div class="code__body">
                            <v-code-editor
                                class="v-code-editor"
                                v-model="sourceCode"
                            ></v-code-editor>
                        </div>
                        <div class="code__footer">

                        </div>
                    </div>
                </v-splitter-panel>
            </v-splitter>
        </div>
        <div class="problem-detail__footer">

        </div>
    </div>
</template>
<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { problemService, languageService } from "@/services/services";
import { useLanguageStore } from "@/stores/stores";
import { mapStores, mapState } from 'pinia';
import problemEnum from "@/enums/problem-enum";
import problemConst from "@/consts/problem-const.js";
import ParameterItem from "./ParameterItem.vue";
import TestcaseItem from "./TestcaseItem.vue";

export default {
    name: "ProblemDetail",
    extends: BaseForm,
    components: {
        ParameterItem,
        TestcaseItem,
    },
    data() {
        return {
            sourceCode: 'Hello world',
            instanceService: problemService,
            problemConst: problemConst,
            languages: [],
            selectedLanguage: {},
            selectedTimeUnit: this.$cv.enumToSelects(this.$enums.timeUnit)[0],
            selectedMemoryUnit: this.$cv.enumToSelects(this.$enums.memoryUnit)[0],
        }
    },
    mounted() {
        var a = this.$route.params.id;
    },
    computed: {
        ...mapStores(useLanguageStore),
    },
    methods: {
        /**
         * Lấy dữ liệu
         */
        async loadDataOnCreated() {
            await this.getLanguages();
            if (this.$cf.isEmptyObject(this.selectedLanguage) && !this.$cf.isEmptyArray(this.languages)) {
                this.selectedLanguage = this.languages[0];
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
        }
    }
}
</script>
<style scoped>
@import './problem-detail.css';
</style>