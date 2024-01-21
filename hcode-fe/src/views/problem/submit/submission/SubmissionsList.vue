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
                >
                    <!-- :isSelected="isSelected(item[itemId])" -->
                    <template #content>
                        <!-- Trạng thái thi -->
                        <!-- <v-td
                            :content="$cv.enumToResource(item.ContestAccountState, problemEnum.problemAccountState)"
                            :style="{
                                color: $cv.problemAccountStateToColor(item.ContestAccountState),
                                fontWeight: 700,
                            }"
                        >
                        </v-td> -->
                        <!-- Mã -->
                        <!-- <v-td
                            :style="{
                                color: $enums.color.yellow,
                                fontWeight: 700,
                            }"
                            :content="item.ContestCode"
                        >
                        </v-td> -->
                        <!-- Tên -->
                        <!-- <v-td
                            :style="{
                                color: $enums.color.yellow,
                                fontWeight: 700,
                            }"
                            :content="item.ContestName"
                        >
                        </v-td> -->
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
import BaseList from "@/components/base/BaseList.vue";
import { submissionService } from "@/services/services.js";
import problemEnum from "@/enums/problem-enum.js";

export default {
    name: "SubmissionList",
    extends: BaseList,
    props: {
        parentId: {
            type: [String],
            default: null,
        }
    },
    data() {
        return {
            documentTitle: this.$t("problem.problemList"),
            itemIdKey: "ContestId",
            problemEnum: problemEnum,
            /**
             * Các cột
             */
            columns: [
                {
                    title: this.$t("problem.field.createdTime"),
                    textAlign: 'left',
                    widthCell: 100,
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
                    widthCell: 200,
                    name: "StatusName",
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
                    widthCell: 100,
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
                    widthCell: 100,
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
                    name: "LanguageName",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
            ],
            problemStates: [],
            problemFilters: [],
            dateTimePattern: 'dd/mm/yyyy hh/mm',
        }
    },
    computed: {
        /**
         * Thêm lọc theo State
         */
        addFilterModelsComputed() {
            let filters = [];

             let filterPrivate = [
                {
                    columnName: 'ParentId',
                    logicType: this.$enums.logicType.and,
                    compareType: this.$enums.compareType.equal,
                    filterValue: this.parentId,
                },
            ];
            filters.push(...filterPrivate);

            return filters;
        },
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
            this.problemFilters = this.$cv.enumToSelects(problemEnum.problemFilter);
            this.selectedContestFilter = this.problemFilters[0];
        },
        /**
         * Click vào nút tạo mới
         */
        clickCreate() {
            this.$router.push(this.$path.problem)
        },
        /**
         * Chọn problem state
         */
        onSelectedContestFilter() {
            this.reloadItems();
        },
    }
}
</script>
<style scoped>
@import './submissions-list.css';
</style>
