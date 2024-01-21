<template>
    <div class="problems-list-container">
        <div class="problems-list__left">
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
                        style="width: 160px;"
                    >
                        <v-combobox
                            v-model="selectedProblemState"
                            optionLabel="name"
                            :options="problemStates"
                            @change="onSelectedProblemState"
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
                            <!-- Trạng thái -->
                            <v-td :style="{
                                textAlign: 'center'
                            }">
                                <v-icon
                                    :icon="$cv.problemAccountStateToIcon(item.ProblemAccountState)"
                                    :color="$cv.problemAccountStateToColor(item.ProblemAccountState)"
                                    :tooltip="$cv.enumToResource(item.ProblemAccountState, problemEnum.problemAccountState)"
                                />
                            </v-td>
                            <!-- Tên -->
                            <v-td
                                :style="{
                                    color: $enums.color.yellow,
                                    fontWeight: 700,
                                }"
                                :content="item.ProblemName"
                            >
                                <v-tag
                                    v-if="item.IsNew"
                                    value="New"
                                    severity="danger"
                                ></v-tag>
                            </v-td>
                            <!-- Chủ đề -->
                            <v-td :content="item.Topic">
                            </v-td>
                            <!-- Độ khó-->
                            <v-td
                                :content="$cv.enumToResource(item.Difficulty, $enums.difficulty)"
                                :style="{
                                    color: $cv.difficultyToColor(item.Difficulty),
                                    fontWeight: 700
                                }"
                            >
                            </v-td>
                            <!-- Đã xem -->
                            <v-td :style="{
                                textAlign: 'center'
                            }">
                                <div style="width: 32px">
                                    {{ $cv.numberToSuffix(item.SeenCount) }}
                                </div>
                                <v-icon
                                    icon='far fa-eye'
                                    color="light"
                                ></v-icon>
                            </v-td>
                            <!-- Tỉ lệ đúng -->
                            <v-td :style="{
                                textAlign: 'center',
                                // color: $enums.color.green
                            }">
                                <div style="width: 56px">
                                    {{ $cv.rateToPercent(item.AcceptedRate, 1, '0%') }}
                                </div>
                                <v-icon
                                    icon='far fa-circle-check'
                                    color="success"
                                ></v-icon>
                            </v-td>
                            <!-- Chức năng -->
                            <v-td :style="{
                                textAlign: 'center'
                            }">
                                <div class="flex-align-center col-gap-4">
                                    <v-button
                                        v-if="item.IsDraft == false"
                                        icon="far fa-code"
                                        severity="info"
                                        text
                                        raised
                                        rounded
                                        :title="$t('problem.practice')"
                                        @click="clickRun(item.ProblemId)"
                                    />
                                    <v-button
                                        icon="far fa-pen"
                                        severity="warning"
                                        text
                                        raised
                                        rounded
                                        :title="$t('com.edit')"
                                        @click="clickEdit(item.ProblemId)"
                                    />
                                    <v-button
                                        icon="far fa-trash"
                                        severity="danger"
                                        text
                                        raised
                                        rounded
                                        :title="$t('com.delete')"
                                        @click="clickDelete(item.ProblemId, item.ProblemName)"
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
        <!-- <div class="problems-list__right">
            <div class="problems-list__stat">

            </div>
        </div> -->
    </div>
</template>
<script>
import BaseList from "@/components/base/BaseList.vue";
import { problemService } from "@/services/services.js";
import problemEnum from "@/enums/problem-enum.js";

export default {
    name: "ProblemsList",
    extends: BaseList,
    data() {
        return {
            documentTitle: this.$t("problem.problemList"),
            itemIdKey: "ProblemId",
            cfg: {
                formPath: this.$path.problem,
                subSysName: this.$t("problem.problem")
            },
            problemEnum: problemEnum,
            /**
             * Các cột
             */
            columns: [
                {
                    title: this.$t("problem.field.status"),
                    textAlign: 'left',
                    widthCell: 60,
                    name: "ProblemAccountState",
                    filterConfig: {
                        filterType: this.$enums.filterType.selectKey,
                        selects: this.$cv.enumToSelects(problemEnum.problemAccountState),
                    }
                },
                {
                    title: this.$t("problem.field.problemName"),
                    textAlign: 'left',
                    widthCell: 180,
                    name: "ProblemName",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                        field: "ProblemCode"
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.field.topic"),
                    textAlign: 'left',
                    widthCell: 140,
                    name: "Topic",
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.field.difficulty"),
                    textAlign: 'left',
                    widthCell: 60,
                    name: "Difficulty",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.selectKey,
                        selects: this.$cv.enumToSelects(this.$enums.difficulty),
                    }
                },
                {
                    title: this.$t("problem.field.seen"),
                    textAlign: 'center',
                    widthCell: 80,
                    name: "SeenCount",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                    },
                    filterConfig: {
                        filterType: this.$enums.filterType.number,
                    }
                },
                {
                    title: this.$t("problem.field.acceptedRate"),
                    textAlign: 'center',
                    widthCell: 80,
                    name: "AcceptedRate",
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
                    widthCell: 60,
                }
            ],
            problemStates: [],
            selectedProblemState: null,
        }
    },
    computed: {
        /**
         * Thêm lọc theo State
         */
        addFilterModelsComputed() {
            let filters = [];

            // Lọc thêm State
            switch (this.$cv.selectedToEnumKey(this.selectedProblemState)) {
                // Công khai
                case problemEnum.problemState.public.value:
                    let filterPublic = {
                        columnName: 'State',
                        logicType: this.$enums.logicType.and,
                        compareType: this.$enums.compareType.equal,
                        filterValue: problemEnum.problemState.public.value
                    };
                    filters.push(filterPublic);
                    break;
                // Cá nhân
                case problemEnum.problemState.private.value:
                    let filterPrivate = [
                        {
                            columnName: 'State',
                            logicType: this.$enums.logicType.and,
                            compareType: this.$enums.compareType.equal,
                            filterValue: problemEnum.problemState.private.value
                        },
                        {
                            columnName: 'IsDraft',
                            logicType: this.$enums.logicType.and,
                            compareType: this.$enums.compareType.equal,
                            filterValue: 0,
                        },
                        {
                            columnName: 'AccountId',
                            logicType: this.$enums.logicType.and,
                            compareType: this.$enums.compareType.accountId,
                        }
                    ];
                    filters.push(...filterPrivate);
                    break;
                // Nháp
                case problemEnum.problemState.draft.value:
                    let filterDraft = [
                        {
                            columnName: 'IsDraft',
                            logicType: this.$enums.logicType.and,
                            compareType: this.$enums.compareType.equal,
                            filterValue: 1,
                        },
                        {
                            columnName: 'AccountId',
                            logicType: this.$enums.logicType.and,
                            compareType: this.$enums.compareType.accountId,
                        }
                    ];
                    filters.push(...filterDraft);
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
         */
        initOnCreated() {
            this.itemService = problemService;
            this.problemStates = this.$cv.enumToSelects(problemEnum.problemState);
            this.selectedProblemState = this.problemStates[1];
        },
        /**
         * Chọn problem state
         */
        onSelectedProblemState() {
            this.reloadItems();
        },
        /**
         * Click submit
         */
        clickRun(id) {
            if (id != null) {
                this.$router.push(this.$cf.combineRoute(this.$path.problemSubmit, id))
            }
        }
    }


}
</script>
<style scoped>
@import './problems-list.css';
</style>
