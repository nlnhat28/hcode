<script>
import BaseProblemDetail from "@/views/problem/base/BaseProblemDetail.vue";
import { problemService } from "@/services/services";
import problemEnum from "@/enums/problem-enum";
import problemConst from "@/consts/problem-const.js";
import ParameterItem from "./ParameterItem.vue";
import TestcaseItem from "./TestcaseItem.vue";
import enums from "@/enums/enums";

const formMode = enums.formMode;

export default {
    name: "BaseSubmit",
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
            allowBuildSource: true,
        }
    },
    watch: {
        "instance.SolutionLanguage": {
            handler() {
                if (this.checkBuildSource) {
                    this.buildSourceCode();
                }
                this.allowBuildSource = true;
            },
            deep: true
        },
    },
    mounted() {

    },
    computed: {
        checkBuildSource() {
            return this.allowBuildSource || this.$cf.isEmptyString(this.instance.Solution)
        },
    },
    methods: {
        /**
         * Khởi tạo dữ liệu data
         */
        async initOnCreated() {
            this.difficulties = this.$cv.enumToSelects(enums.difficulty);
            this.dataTypes = this.$cv.enumToSelects(problemEnum.dataType);

            this.mode = formMode.view;
        },
        /** 
         * @override
         */
        customInstanceOnCreated() {
            this.selectedOutputType = this.$cv.enumKeyToSelected(this.instance.OutputType, this.dataTypes, 0);
            this.selectedDifficulty = this.$cv.enumKeyToSelected(this.instance.Difficulty, this.difficulties, 0);

            if (!this.$cf.isEmptyArray(this.languages)) {
                this.instance.SolutionLanguage = this.languages[0];
            }
        },
        /**
         * Click nộp
         */
        async clickSubmit() {
            if (this.checkAuthenticated()) {
                if (this.$cf.isEmptyString(this.instance.Solution)) {
                    let msg = this.$t('msg.labelCannotNull', { label: this.$t('problem.field.solution') });
                    this.$dl.error(msg);
                }
                else {
                    this.resetTestcaseStatus();
                    await this.loadingEffect(this.submit);
                }
            }
        },
        async submit() {
            try {
                const response = await this.instanceService.submit(
                    this.reformatInstance()
                );
                if (this.$res.isSuccess(response)) {
                    this.$ts.success();
                } else {
                    this.handleError(response);
                }
                this.processSubmissionResponse(response);

                if (this.$res.hasSuccessCode(response, this.$res.successCode.SubmissionSaved)) {
                    this.reloadSubmissionList();
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * reload SubmissionList
         */
        reloadSubmissionList() {
            let ref = this.$refs.refSubmissionList;
            if (ref && typeof (ref.reloadItems) == 'function') {
                ref.reloadItems();
            }
        },
        /**
         * Bind submission
         */
        bindSubmission(submit) {
            let solutionLanguage = this.languages.find(l => l.LanguageId == submit.LanguageId)
            if (solutionLanguage) {
                this.instance.SolutionLanguage = solutionLanguage;
                this.allowBuildSource = false;
            }
            if (!this.$cf.isEmptyString(submit.SourceCode)) {
                this.instance.Solution = submit.SourceCode;
                // this.allowBuildSource = true;
            }
        }
    }
}
</script>