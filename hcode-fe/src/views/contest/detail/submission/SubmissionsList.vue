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
                    style="width: 180px;"
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
                    @doubleClick="onSelect(item)"
                >
                    <template #content>
                        <!-- Ngày tạo -->
                        <v-td
                            :content="$fm.formatDateTime(item.CreatedTime, dateTimePattern)"
                            :style="{ textAlign: 'center' }"
                        >
                        </v-td>
                        <!-- Trạng thái -->
                        <v-td
                            :content="item.StatusName"
                            :style="{ color: $cv.statusJudge0ToColor(item.StatusId)}"
                        >
                        </v-td>
                        <!-- Thời gian -->
                        <v-td
                            :content="item.RunTime != null ? item.RunTime + ' s' : '_'"
                        >
                        </v-td>
                        <!-- Bộ nhớ -->
                        <v-td
                            :content="item.Memory != null ? item.Memory + ' kb' : '_'"
                        >
                        </v-td>
                        <!-- Ngôn ngữ -->
                        <v-td
                            :content="item.LanguageName"
                        >
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
</template>
<script>
import problemEnum from "@/enums/problem-enum.js";
import contestEnum from "@/enums/contest-enum.js";
import BaseSubmissionList from "@/view/submission/BaseSubmissionList.vue";
import { submissionService } from "@/services/services.js";

export default {
    name: "SubmissionList",
    extends: BaseSubmissionList,
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
                    widthCell: 140,
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
                    name: "AccountName",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
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
        }
    },
    watch: {
        // contestProblems: {
        //     handler() {

        //     },
        //     deep: false,
        // }
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
                selects.forEach(s => s.DisplayName = `${this.$t('contest.questionSimple')} ${s.Order}: ${this.ProblemName}`)
            };

            let allItem = {
                IsAll: true,
                ContestProblemId: -1,
                DisplayName: this.$t('com.all'),
            }

            selects.push(allItem);
            return selects;
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
                this.selectedContestFilter = this.contestProblemsSelects[0];
            }
        },
        /**
         * Thay đổi contest problem
         */
        onSelectedContestProblem() {
            this.reloadItems();
        }
    }
}
</script>
<style scoped>
@import './submissions-list.css';
</style>
