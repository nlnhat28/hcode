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
                        <v-td
                            :content="$cv.enumToResource(item.State, contestEnum.contestAccountState)"
                            :style="{
                                color: $cv.contestAccountStateToColor(item.State),
                                fontWeight: 700,
                            }"
                        >
                        </v-td>
                        <!-- Tên -->
                        <v-td
                            :style="{
                                color: $enums.color.yellow,
                                fontWeight: 700,
                            }"
                            :content="item.FullName"
                        >
                        </v-td>
                        <!-- Ngày tạo -->
                        <v-td
                            :content="$fm.formatDateTime(item.StartTime, dateTimePattern)"
                            :style="{ textAlign: 'center' }"
                        ></v-td>
                        <!-- Điểm -->
                        <v-td
                            :style="{
                                color: $enums.color.green,
                                textAlign: 'center',
                                fontWeight: 700,
                            }"
                            :content="item.State == contestEnum.contestAccountState.pending.value ? '_' : item.TotalScore"
                        >
                        </v-td>
                        <!-- Chức năng -->
                        <v-td :style="{
                            textAlign: 'center'
                        }">
                            <div class="flex-align-center col-gap-4">
                                <v-button
                                    v-if="1"
                                    icon="far fa-table"
                                    severity="info"
                                    text
                                    raised
                                    rounded
                                    :title="$t('contest.viewResult')"
                                    @click="clickJoin(item.ContestId)"
                                />
                            </div>
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
import BaseList from "@/components/base/BaseList.vue";
import { contestService, contestAccountService } from "@/services/services.js";
import contestEnum from "@/enums/contest-enum.js";

export default {
    name: "ContestsList",
    extends: BaseList,
    data() {
        return {
            documentTitle: this.$t("contest.contestList"),
            itemIdKey: "ContestAccountId",
            contestEnum: contestEnum,
            hasBuildDocumentTitle: false,
            /**
             * Các cột
             */
            columns: [
                {
                    // Trạng thái
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
                    // Tên người tham gia
                    title: this.$t("contest.field.fullName"),
                    textAlign: 'left',
                    widthCell: 160,
                    name: "FullName",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("contest.field.startTime"),
                    textAlign: 'center',
                    widthCell: 120,
                    name: "StartTime",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    // Tổng điểm
                    title: this.$t("contest.field.score"),
                    textAlign: 'center',
                    widthCell: 100,
                    name: "TotalScore",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.number,
                    }
                },
                {
                    title: this.$t("com.function"),
                    textAlign: 'center',
                    widthCell: 80,
                }
            ],
        }
    },
    computed: {
        /**
         * Thêm lọc theo State
         */
        addFilterModelsComputed() {

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
            this.itemService = contestAccountService;
        },
    }
}
</script>
<style scoped>
@import './contest-account-list.css';
</style>
