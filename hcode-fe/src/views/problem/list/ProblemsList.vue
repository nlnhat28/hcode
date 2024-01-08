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
                    <div class="dark" style="width: 160px;">
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
                            <!-- Tương tấc -->
                            <v-td :style="{
                                textAlign: 'center'
                            }">
                                <v-reaction
                                    :rate="item.Rate"
                                    :comment="item.CommentCount"
                                    :seen="item.SeenCount"
                                />
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
        <div class="problems-list__right">
            <div class="problems-list__stat">

            </div>
        </div>
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
                    widthCell: 200,
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
                    title: this.$t("problem.field.reaction"),
                    textAlign: 'center',
                    widthCell: 100,
                    name: "Reaction",
                    sortConfig: {
                        sortType: this.$enums.sortType.blur,
                        field: "ReactionScore",
                    }
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
            filters.push({
                columnName: 'State',
                logicType: this.$enums.logicType.and,
                logicName: 'and',
                compareType: this.$enums.compareType.equal,
                compareName: '=',
                filterValue: this.selectedProblemState?.key
            });

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
            this.selectedProblemState = this.problemStates[0];
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
        onSelectedProblemState() {
            this.reloadItems();
        },
        // addFilterProblemState() {

        //     if (!this.$cf.isEmptyArray(this.filterModels)) {
        //         let filterProblemState = this.filterModels.find(item => item.column == 'State')
        //         if (filterProblemState) {
        //             filterProblemState.values = [this.selectedProblemState];
        //             return;
        //         }
        //     }
        //     this.filterModels.push({
        //         column: 'State',
        //         logicType: this.$enums.logicType.and,
        //         logicName: 'and',
        //         compareType: this.$enums.compareType.equal,
        //         compareName: '=',
        //         values: [this.selectedProblemState]
        //     });
        // }
    }


}
</script>
<style scoped>
@import './problems-list.css';
</style>
