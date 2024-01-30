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
            <!-- <template #toolbarLeft>
                <div
                    class="dark"
                    style="width: 180px;"
                >
                    <v-combobox
                        v-model="selectedContestFilter"
                        optionLabel="name"
                        :options="problemFilters"
                        @change="onSelectedContestFilter"
                    />
                </div>
            </template> -->
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
                    @click="onSelect(item)"
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
</template>
<script>
import problemEnum from "@/enums/problem-enum.js";
import contestEnum from "@/enums/contest-enum.js";
import BaseSubmissionList from "@/views/submission/BaseSubmissionList.vue";

export default {
    name: "SubmissionList",
    extends: BaseSubmissionList,
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
        }
    },
    computed: {
        /**
         * Thêm lọc theo ParentId
         */
        addFilterModelsComputed() {
            let filters = [];

            let filterPrivate = [
                {
                    columnName: 'ProblemAccountId',
                    logicType: this.$enums.logicType.or,
                    compareType: this.$enums.compareType.equal,
                    filterValue: this.parentId,
                },
                {
                    columnName: 'ContestProblemAccountId',
                    logicType: this.$enums.logicType.or,
                    compareType: this.$enums.compareType.equal,
                    filterValue: this.parentId,
                },
            ];
            filters.push(...filterPrivate);

            return filters;
        },
    },
}
</script>
<style scoped>
@import './submissions-list.css';
</style>
