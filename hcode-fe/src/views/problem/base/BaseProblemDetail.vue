
<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { problemService, languageService } from "@/services/services";
import { useLanguageStore, useProblemStore } from "@/stores/stores";
import { mapStores, mapState } from 'pinia';
import problemEnum from "@/enums/problem-enum";
import problemConst from "@/consts/problem-const.js";
import enums from "@/enums/enums";

const formMode = enums.formMode;

export default {
    name: "BaseProblemDetail",
    extends: BaseForm,
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
                        sourceCode = `class ${className} {\npublic:\n${tab}${outputCode} ${funcName}(${paramCode}) {\n${tab2}\n${tab}}\n};`;
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
                        sourceCode = `class ${className}():\n${tab}def ${funcName}(self, ${paramCode}):\n${tab2}\n`;
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
                    // Xoá
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
                    // Xoá
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