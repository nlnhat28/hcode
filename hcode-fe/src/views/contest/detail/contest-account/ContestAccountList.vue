<template>
    <div class="contest-account-container">
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
                        :options="contestFilters"
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
                            :content="$cv.enumToResource(item.ContestAccountState, contestEnum.contestAccountState)"
                            :style="{
                                color: $cv.contestAccountStateToColor(item.ContestAccountState),
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
                    title: this.$t("contest.field.fullName"),
                    textAlign: 'left',
                    widthCell: 200,
                    name: "FullName",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("contest.field.score"),
                    textAlign: 'left',
                    widthCell: 100,
                    name: "Score",
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
            dateTimePattern: 'dd/mm/yyyy hh/mm',
        }
    },
    computed: {
        /**
         * Thêm lọc theo State
         */
        addFilterModelsComputed() {
            let filters = [];

            let filterKey = this.$cv.selectedToEnumKey(this.selectedContestFilter);

            switch (filterKey) {
                // Công khai
                case contestEnum.contestFilter.public.value:
                    filters.push({
                        columnName: 'IsPublic',
                        logicType: this.$enums.logicType.and,
                        compareType: this.$enums.compareType.equal,
                        filterValue: 1
                    });
                    break;
                // Cá nhân
                case contestEnum.contestFilter.private.value:
                    filters.push({
                        columnName: 'AccountId',
                        logicType: this.$enums.logicType.and,
                        compareType: this.$enums.compareType.equal,
                        filterValue: this.$auth.getAccountId()
                    });
                    break;
                // Đã tham gia
                case contestEnum.contestFilter.joined.value:
                    filters.push({
                        columnName: 'ContestAccountState',
                        logicType: this.$enums.logicType.and,
                        compareType: this.$enums.compareType.notNull,
                    });
                    break;

                default:
                    break;
            }

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
    }
}
</script>
<style scoped>
@import './contest-account-list.css';
</style>
