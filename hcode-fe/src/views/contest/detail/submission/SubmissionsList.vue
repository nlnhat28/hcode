<template>
    <div class="submission-list-container">
        <v-table
            :itemId="itemIdKey"
            :resizable="false"
            :columns="columns"
            :isLoading="isLoading"
            :totalRecord="totalRecord"
            :sortModels="sortModels"
            :filterModels="filterModels"
        >
            <template #toolbarLeft>
                <div
                    class="dark"
                    style="width: 360px;"
                >
                    <!-- Lọc theo danh sách câu hỏi -->
                    <v-combobox
                        v-model="selectedContestProblem"
                        optionLabel="DisplayName"
                        :options="contestProblemsSelects"
                        @change="onSelectedContestProblem"
                    />
                </div>
            </template>
            <template #toolbarRight>
                <v-search-box
                    style="width: 280px;"
                    v-model="keySearch"
                    ref="refSearchBox"
                ></v-search-box>
                <v-button
                    icon="far fa-arrows-rotate"
                    severity="secondary"
                    outlined
                    :loading="isLoading"
                    :title="$t('com.reload')"
                    @click="reloadItems()"
                />
            </template>
            <template #tbody>
                <v-tr
                    ref="tr"
                    v-for="(item, index) in items"
                    :key="item[itemIdKey] ?? index"
                    :index="index"
                    :id="item[itemIdKey]"
                    @click="showSubmissionDialog(item)"
                >
                    <template #content>
                        <!-- Ngày tạo -->
                        <v-td
                            :content="$fm.formatDateTime(item.CreatedTime, dateTimePattern)"
                            :style="{ textAlign: 'center' }"
                        >
                        </v-td>
                        <!-- Người nộp -->
                        <v-td :content="item.FullName">
                        </v-td>
                        <!-- Trạng thái -->
                        <v-td
                            :content="item.StatusName"
                            :style="{ color: $cv.statusJudge0ToColor(item.StatusId) }"
                        >
                        </v-td>
                        <!-- Thời gian -->
                        <v-td :content="item.RunTime != null ? item.RunTime + ' s' : '_'">
                        </v-td>
                        <!-- Bộ nhớ -->
                        <v-td :content="item.Memory != null ? item.Memory + ' kb' : '_'">
                        </v-td>
                        <!-- Ngôn ngữ -->
                        <v-td :content="item.LanguageName">
                        </v-td>
                    </template>
                </v-tr>
            </template>
            <template #tfooter>
                <!-- Phân trang -->
                <v-pagination
                    :totalRecord="totalRecord"
                    v-model:pagingModel="pagingModel"
                    canAccessRandom
                    isShowPageNumbers
                    @emitUpdatePage="updatePage"
                >
                </v-pagination>
            </template>
        </v-table>
    </div>
    <v-dialog
        class="v-dialog dark submission-dialog"
        v-model:visible="isShowSubmissionDialog"
        modal
        :header="headerSubmission"
        :draggable="false"
        style="width: 1200px; height: 800px;"
    >
        <template #closeIcon>
            <v-icon icon="far fa-circle-xmark" />
        </template>
        <div class="v-dialog__content">
            <v-tab-view>
                <!-- Lời giải -->
                <v-tab-panel :header="$t('problem.field.solution')">
                    <div class="w-full flex-column row-gap-20 p-20">
                        <v-code-mirror
                            v-model="selectedSubmission.SourceCode"
                            :language="languageIdSubmission"
                        ></v-code-mirror>
                    </div>
                </v-tab-panel>
                <!-- Testcase -->
                <v-tab-panel :header="$t('problem.test')">
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
        <template #footer>
            <v-button-container justifyContent="space-between">
                <!-- Các nút bên trái -->
                <div>
                    <v-button
                        outlined
                        :label="$t('com.close')"
                        @click="closeSubmissionDialog"
                    ></v-button>
                </div>
                <!-- Các nút bên phải -->
                <div class="flex col-gap-8">
                    <!-- Đóng -->
                    <v-button
                        v-if="selectedSubmission.SourceCode"
                        :label="$t('com.copy')"
                        @click="copySubmission"
                    ></v-button>
                </div>
            </v-button-container>
        </template>
    </v-dialog>
</template>
<script>
import problemEnum from "@/enums/problem-enum.js";
import contestEnum from "@/enums/contest-enum.js";
import BaseSubmissionList from "@/views/submission/BaseSubmissionList.vue";
import { submissionService } from "@/services/services.js";
import SubmissionDialog from "@/views/submission/SubmissionDialog.vue";
import ParameterItem from "@/views/submit/ParameterItem.vue";
import TestcaseItem from "@/views/submit/TestcaseItem.vue";

export default {
    name: "SubmissionList",
    extends: BaseSubmissionList,
    components: {
        SubmissionDialog
        ParameterItem,
        TestcaseItem,
    },
    props: {
        contestId: {
            type: [String],
            default: null,
        },
        contestProblemId: {
            type: [String],
            default: null,
        },
        contestProblems: {
            type: [Array],
            default: [],
        }
    },
    data() {
        return {
            /**
             * Các cột
             */
            columns: [
                {
                    title: this.$t("problem.field.createdTime"),
                    textAlign: 'center',
                    widthCell: 120,
                    name: "CreatedTime",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.field.submitFullName"),
                    textAlign: 'left',
                    widthCell: 120,
                    name: "FullName",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.field.statusName"),
                    textAlign: 'left',
                    widthCell: 100,
                    name: "StatusId",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.selectKey,
                        selects: this.statusJudge0s = this.$cv.enumToSelects(problemEnum.statusJudge0),
                    }
                },
                {
                    title: this.$t("problem.field.runTime"),
                    textAlign: 'left',
                    widthCell: 80,
                    name: "RunTime",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.number,
                    }
                },
                {
                    title: this.$t("problem.field.memory"),
                    textAlign: 'left',
                    widthCell: 80,
                    name: "Memory",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.number,
                    }
                },
                {
                    title: this.$t("problem.field.languageName"),
                    textAlign: 'left',
                    widthCell: 100,
                    name: "LanguageId",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.selectKey,
                        selects: this.languages,
                    }
                },
            ],
            selectedContestProblem: {},
            submissionDialog: {
                isShow: false,
                object: null,
            },
            isShowSubmissionDialog: false,
            selectedSubmission: {},
            languageIdSubmission: {}
        }
    },
    watch: {
        contestId() {
            this.reloadItems();
        },
    },
    computed: {
        /**
         * Thêm lọc theo ParentId
         */
        addFilterModelsComputed() {
            let filters = [];

            if (this.selectedContestProblem.IsAll) {
                let filterContest = [
                    {
                        columnName: 'ContestId',
                        logicType: this.$enums.logicType.and,
                        compareType: this.$enums.compareType.equal,
                        filterValue: this.contestId,
                    },
                ];
                filters.push(...filterContest);
            }
            else if (this.selectedContestProblem && this.selectedContestProblem.ContestProblemId) {
                let filterContestProblem = [
                    {
                        columnName: 'ContestProblemId',
                        logicType: this.$enums.logicType.and,
                        compareType: this.$enums.compareType.equal,
                        filterValue: this.selectedContestProblem.ContestProblemId,
                    },
                ];
                filters.push(...filterContestProblem);
            }

            return filters;
        },
        /**
         * Danh sách câu hỏi
         */
        contestProblemsSelects() {
            let selects = [];

            if (this.contestProblems) {
                selects = this.contestProblems.filter(c => c.ProblemId != null);
                selects.forEach(s => s.DisplayName = `${this.$t('contest.questionSimple')} ${s.Order}: ${s.ProblemName}`)
            };

            let allItem = {
                IsAll: true,
                ContestProblemId: -1,
                DisplayName: this.$t('contest.allQuestion'),
            }

            selects.unshift(allItem);

            return selects;
        },
        headerSubmission() {
            if (this.selectedSubmission) {
                let s = this.selectedSubmission;
                return `${this.$t('problem.field.solution')} - ${s.FullName} - ${s.LanguageName}`
            }
            return ''
        }
    },
    mounted() {
    },
    methods: {
        /**
         * Override
         * 
         */
        initOnCreated() {
            this.itemService = submissionService;

            if (!this.$cf.isEmptyArray(this.contestProblemsSelects)) {
                this.selectedContestProblem = this.contestProblemsSelects[0];
            }
        },
        /**
         * Thay đổi contest problem
         */
        onSelectedContestProblem() {
            this.reloadItems();
        },
        /**
         * Hiện submission dialog
         * @param {*} item 
         */
        showSubmissionDialog(item) {
            this.selectedSubmission = item;
            this.isShowSubmissionDialog = true;
            this.languageIdSubmission = this.languages.find(l => l.LanguageId == item.LanguageId)?.JudgeId ?? problemEnum.language.csharp
        },
        closeSubmissionDialog() {
            this.isShowSubmissionDialog = false;
        },
        copySubmission() {
            this.$cf.copyToClipboard(this.selectedSubmission.SourceCode);
            this.$ts.success(this.$t("com.copied"));
        }
    }
}
</script>
<style scoped>
@import './submissions-list.css';
</style>
