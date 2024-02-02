
<script>
import BaseProblemDetail from "@/views/problem/base/BaseProblemDetail.vue";
import { problemService, languageService, contestService } from "@/services/services";
import { useLanguageStore, useProblemStore } from "@/stores/stores";
import { mapStores, mapState } from 'pinia';
import problemEnum from "@/enums/problem-enum";
import contestEnum from "@/enums/contest-enum";
import problemConst from "@/consts/problem-const.js";
import ParameterItem from "./ParameterItem.vue";
import TestcaseItem from "./TestcaseItem.vue";
import SubmissionsList from "./submission/SubmissionsList.vue";
import ContestSiderbar from "./sidebar/ContestSidebar.vue"
import enums from "@/enums/enums";
import BaseSubmissionList from "../views/submission/BaseSubmissionList.vue";

const formMode = enums.formMode;

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
                    this.countdownText = this.$cf.countdown(
                        null,
                        new Date(),
                        endTime,
                        null,
                        ["m", "s"],
                        "short",
                        true
                    ).text;
                }, 1000)
            }
        },
    }
}
</script>