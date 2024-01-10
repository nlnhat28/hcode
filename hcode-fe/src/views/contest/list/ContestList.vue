<template>
    <div class="contests-list-container">
        <div class="contests-list__left">
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
                        <v-combobox
                            v-model="selectedContestFilter"
                            optionLabel="name"
                            :options="contestFilters"
                            @change="onSelectedContestFilter"
                        />
                    </div>
                </template>
                <template #toolbarRight>
                    <v-search-box
                        v-model="keySearch"
                        ref="refSearchBox"
                    ></v-search-box>
                    <div v-tooltip="$t('com.reload')">
                        <v-button
                            icon="far fa-arrows-rotate"
                            severity="secondary"
                            outlined
                            :loading="isLoading"
                            @click="reloadItems()"
                        />
                    </div>
                    <v-button
                        icon="fa fa-plus"
                        :label="$t('com.createNew')"
                        @click="clickCreate()"
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
                            <v-td
                                :content="$cv.enumToResource(item.ContestAccountState, contestEnum.contestAccountState)"
                                :style="{
                                    color: $cv.contestAccountStateToColor(item.ContestAccountState),
                                }"
                            >
                            </v-td>
                            <!-- Mã -->
                            <v-td
                                :style="{
                                    color: $enums.color.yellow,
                                    fontWeight: 700,
                                }"
                                :content="item.ContestCode"
                            >
                            </v-td>
                            <v-td
                                :style="{
                                    color: $enums.color.yellow,
                                    fontWeight: 700,
                                }"
                                :content="item.ContestName"
                            >
                            </v-td>
                            <!-- Trạng thái -->
                            <v-td
                                :content="$cv.enumToResource(item.State, contestEnum.contestState)"
                                :style="{
                                    color: $cv.contestStateToColor(item.State),
                                }"
                            >
                            </v-td>
                            <!-- Thời gian bắt đầu -->
                            <v-td :content="item.StartTime"></v-td>
                            <!-- Thời gian kết thúc -->
                            <v-td :content="item.EndTime"></v-td>
                            <!-- Thời gian làm -->
                            <v-td :content="(item.TimeToDo ? `${item.TimeToDo} ${$t('com.minute')}` : '')"></v-td>
                            <!-- Đã tham gia -->
                            <v-td :style="{
                                textAlign: 'center'
                            }">
                                {{ $cv.numberToSuffix(item.JoinCount) }}
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
        <div class="contests-list__right">
            <div class="contests-list__stat">

            </div>
        </div>
    </div>
</template>
<script>
import BaseList from "@/components/base/BaseList.vue";
import { contestService } from "@/services/services.js";
import contestEnum from "@/enums/contest-enum.js";

export default {
    name: "ContestsList",
    extends: BaseList,
    data() {
        return {
            documentTitle: this.$t("contest.contestList"),
            itemIdKey: "ContestId",
            contestEnum: contestEnum,
            /**
             * Các cột
             */
            columns: [
                {
                    title: this.$t("contest.field.contestAccountState"),
                    textAlign: 'left',
                    widthCell: 100,
                    name: "ContestAccountState",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.selectKey,
                        selects: this.$cv.enumToSelects(contestEnum.contestAccountState),
                    }
                },
                {
                    title: this.$t("contest.field.contestCode"),
                    textAlign: 'left',
                    widthCell: 100,
                    name: "ContestCode",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("contest.field.contestName"),
                    textAlign: 'left',
                    widthCell: 200,
                    name: "ContestName",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("contest.field.state"),
                    textAlign: 'left',
                    widthCell: 60,
                    name: "State",
                    filterConfig: {
                        filterType: this.$enums.filterType.selectKey,
                        selects: this.$cv.enumToSelects(contestEnum.contestState),
                    }
                },
                {
                    title: this.$t("contest.field.startTime"),
                    textAlign: 'center',
                    widthCell: 100,
                    name: "StartTime",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.dateTime,
                    }
                },
                {
                    title: this.$t("contest.field.endTime"),
                    textAlign: 'center',
                    widthCell: 100,
                    name: "EndTime",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.dateTime,
                    }
                },
                {
                    title: this.$t("contest.field.timeToDo"),
                    textAlign: 'left',
                    widthCell: 100,
                    name: "TimeToDo",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.number,
                    }
                },
                {
                    title: this.$t("contest.field.joinCount"),
                    textAlign: 'left',
                    widthCell: 100,
                    name: "JoinCount",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.number,
                    }
                },
            ],
            contestStates: [],
            contestFilters: [],
            selectedContestFilter: null,
        }
    },
    computed: {
        /**
         * Thêm lọc theo State
         */
        addFilterModelsComputed() {
            let filters = [];

            // Lọc thêm State
            // filters.push({
            //     columnName: 'State',
            //     logicType: this.$enums.logicType.and,
            //     logicName: 'and',
            //     compareType: this.$enums.compareType.equal,
            //     compareName: '=',
            //     filterValue: this.selectedContestFilter?.key
            // });

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
            this.itemService = contestService;
            this.contestFilters = this.$cv.enumToSelects(contestEnum.contestFilter);
            this.selectedContestFilter = this.contestFilters[0];
        },
        /**
         * Click vào nút tạo mới
         */
        clickCreate() {
            this.$router.push(this.$path.contest)
        },
        /**
         * Chọn problem state
         */
        onSelectedContestFilter() {
            this.reloadItems();
        },
        // addFilterProblemState() {

        //     if (!this.$cf.isEmptyArray(this.filterModels)) {
        //         let filterProblemState = this.filterModels.find(item => item.column == 'State')
        //         if (filterProblemState) {
        //             filterProblemState.values = [this.selectedContestFilter];
        //             return;
        //         }
        //     }
        //     this.filterModels.push({
        //         column: 'State',
        //         logicType: this.$enums.logicType.and,
        //         logicName: 'and',
        //         compareType: this.$enums.compareType.equal,
        //         compareName: '=',
        //         values: [this.selectedContestFilter]
        //     });
        // }
    }


}
</script>
<style scoped>
@import './contests-list.css';
</style>
