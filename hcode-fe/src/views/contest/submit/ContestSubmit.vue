<template>
    <div class="contest-submit-container flex">
        <ContestSiderbar
            ref="refSidebar"
            :id="contestId"
            :problemId="instanceId"
            @selected="selectedItem"
            @afterLoad="afterLoadContest"
        ></ContestSiderbar>
        <div class="flex-1 p-relative">
            <!-- Loading -->
            <v-mask-loading v-if="isLoading" />
            <div class="contest-submit__header">
                <div class="contest-submit__header--left flex-align-center">
                    <v-button
                        text
                        severity="secondary"
                        icon="far fa-angle-left"
                        :label="$t('contest.contestList')"
                        @click="$router.push($path.contests)"
                    ></v-button>
                </div>
                <div class="contest-submit__header--center flex-center col-gap-12">
                    <div class="label-20-yellow-300">
                        {{ centerTitle }}
                    </div>
                </div>
                <div class="contest-submit__header--right">

                </div>
            </div>
            <div class="contest-submit__body">
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
                                        :parentId="contestProblemAccountId"
                                        v-if="1"
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
            <div class="contest-submit__footer">
                <v-button-container
                    class="w-full"
                    direction="row-reverse"
                    justifyContent="space-between"
                >
                    <!-- Nộp -->
                    <v-button
                        :label="$t('problem.submitSubmission')"
                        @click="clickSubmit"
                    />
                    <v-button-container
                        direction="row-reverse"
                        :gap="20"
                    >
                        <!-- countdown -->
                        <div
                            class="contest-submit__countdown"
                            v-if="this.contestAccount && timeToDo"
                        >
                            <div class="pb-4">
                                <v-icon
                                    icon="far fa-stopwatch"
                                    color="light"
                                />
                            </div>
                            <div class=" font-18">
                                {{ countdownText }}
                            </div>
                        </div>
                        <!-- toggle ContestSidebar -->
                        <v-button
                            severity="secondary"
                            outlined
                            :icon="'fa fa-grid'"
                            :title="$t('contest.contestInfo')"
                            @click="toggleContestSidebar"
                        />
                    </v-button-container>
                </v-button-container>
            </div>
        </div>
    </div>
</template>
<script>
import BaseProblemSubmit from "@/views/submit/BaseProblemSubmit.vue";
import SubmissionsList from "./submission/SubmissionsList.vue";
import ContestSiderbar from "./sidebar/ContestSidebar.vue"

export default {
    name: "ContestSubmit",
    extends: BaseProblemSubmit,
    components: {
        SubmissionsList,
        ContestSiderbar
    },
    data() {
        return {
            cfg: {
                // formPath: this.$path.contest,
                callbackPath: this.$path.contests,
                entity: 'Contest',
                subSysName: this.$t('contest.contest'),
            },
            hasBuildDocumentTitle: false,
            collapseContestSidebar: false,
            contestId: null,
            contestProblems: null,
            contestAccount: null,
            contestProblemAccountId: null,
            countdownText: null,
            timeToDo: null,
        }
    },
    computed: {
        centerTitle() {
            let title = this.instance?.ProblemName;
            if (!this.$cf.isEmptyArray(this.contestProblems)) {
                let order = this.contestProblems.find(p => p.ProblemId == this.instanceId)?.Order;
                if (order) {
                    title = `${order}. ${title}`;
                }
            };
            return title;
        }
    },
    methods: {
        /**
         * 
         */
        initMode() {
            this.contestId = this.$route.params.contestId;
            this.instanceId = this.$route.params.problemId;
        },
        /**
         * @override
         */
        async loadDataOnCreated() {
            if (this.instanceId && this.instanceId !== '0') {
                await this.viewInstance(this.instanceId);
            }
        },
        async submit() {
            try {
                let contestProblemId = this.contestProblems.find(p => p.ProblemId == this.instanceId)?.ContestProblemId;
                let payload = {
                    ContestProblemId: contestProblemId,
                    ProblemDto: this.reformatInstance()
                }
                const response = await contestService.submit(payload);
                if (this.$res.isSuccess(response)) {
                    this.$ts.success();
                } else {
                    this.handleError(response);
                }
                this.processSubmissionResponse(response);

                let newCpa = this.$res.getDataSuccessCode(response, this.$res.successCode.ContestProblemAccountSaved)
                if (newCpa) {
                    this.reloadContestProblemState(newCpa);
                    this.contestProblemAccountId = newCpa.ContestProblemAccountId;
                }
                if (this.$res.hasSuccessCode(response, this.$res.successCode.SubmissionSaved)) {
                    this.reloadSubmissionList();
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Load lại contest
         */
        reloadContestProblemState(newCpa) {
            if (this.$refs.refSidebar) {
                this.$refs.refSidebar.reloadContestProblemState(newCpa);
            }
        },
        /**
         * Đóng mở contest sidebar
         */
        toggleContestSidebar() {
            if (this.$refs.refSidebar) {
                this.$refs.refSidebar.toggle();
            }
        },
        /**
         * Kết thúc contest
         */
        async finishContest() {
            if (this.$refs.refSidebar) {
                await this.$refs.refSidebar.finishContest();
            }
        },
        /**
         * Gán contestProblemAccountId
         */
        assignContestProblemAccountId(item) {
            this.contestProblemAccountId = this.contestProblems?.find(p => p.ProblemId == item.ProblemId)?.ContestProblemAccountId;

            this.$nextTick(() => {
                this.reloadSubmissionList();
            });
        },
        /**
         * Chọn câu hỏi
         * @param {*} item 
         */
        async selectedItem(item) {
            this.instanceId = item.ProblemId;
            this.$router.push(this.$cf.combineRoute(
                this.$path.contest, this.contestId, this.$path.submit, this.instanceId));
            await this.reloadInstance();
            this.assignContestProblemAccountId(item);
        },
        /**
         * Sau khi load xong contest thì bắn ra danh sách contest problem
         */
        afterLoadContest(contest) {
            this.contestProblems = this.$cf.cloneDeep(contest?.ContestProblems);
            this.contestAccount = this.$cf.cloneDeep(contest?.ContestAccount);
            this.timeToDo = contest?.TimeToDo;

            if (this.contestAccount && contest.TimeToDo) {
                setInterval(() => {
                    let starTime = new Date(this.contestAccount.StartTime);
                    let endTime = starTime.setMinutes(starTime.getMinutes() + contest.TimeToDo);
                    let countdownObj = this.$cf.countdown(
                        null,
                        new Date(),
                        endTime,
                        null,
                        ["m", "s"],
                        "short",
                        true
                    );
                    this.countdownText = countdownObj.text;
                    if (countdownObj.duration <= 0) {
                        this.finishContest();
                    }
                }, 1000)
            }
        },
    }
}
</script>
<style scoped>@import './contest-submit.css';</style>