<template>
    <div class="problems-list-container">
        <div class="problems-list__left">
            <v-table
                itemId="ProblemId"
                :resizable="false"
                :columns="columns"
            >
                <template #tbody>
                    <v-tr
                        v-for="(item, index) in problems"
                        :key="item[itemId] ?? index"
                        :index="index"
                        :id="item[itemId]"
                        ref="tr"
                        @emitUpdate="showMaterialForm(item)"
                        @emitDelete="handleDeleteOnRow(item[itemId])"
                        @emitDuplicate="duplicateMaterial(item)"
                        @emitCreate="showEmptyMaterialForm"
                        @emitSelectMany="selectMany"
                    >
                        <!-- :isSelected="isSelected(item[itemId])" -->
                        <template #content>
                            <!-- Trạng thái -->
                            <v-td :style="{
                                textAlign: 'center'
                            }">
                                <v-icon
                                    :icon="$cv.problemAccountStatusToIcon(item.ProblemAccountStatus)"
                                    :color="$cv.problemAccountStatusToColor(item.ProblemAccountStatus)"
                                    :tooltip="$cv.problemAccountStatusToText(item.ProblemAccountStatus)"
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
                                :content="item.DifficultyName"
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
                                    :comment="item.Comment"
                                    :seen="item.Seen"
                                />
                            </v-td>
                        </template>
                    </v-tr>
                </template>
                <template #tfooter>
                    <!-- Phân trang -->
                    <v-pagination
                        :totalRecord="150"
                        v-model:pageModel="page"
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
import BaseForm from "@/components/base/BaseForm.vue";
import problems from "./problems";

export default {
    name: "ProblemsList",
    extends: BaseForm,
    data() {
        return {
            columns: [
                {
                    title: this.$t("problem.column.status"),
                    textAlign: 'center',
                    widthCell: 60,
                    name: "ProblemAccountStatus",
                    sortType: this.$enums.sortType.disabled,
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.column.problemName"),
                    textAlign: 'left',
                    widthCell: 200,
                    name: "ProblemName",
                    sortType: this.$enums.sortType.blur,
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.column.topic"),
                    textAlign: 'left',
                    widthCell: 120,
                    name: "Topic",
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.column.difficulty"),
                    textAlign: 'left',
                    widthCell: 60,
                    name: "Difficulty",
                    sortType: this.$enums.sortType.blur,
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
                {
                    title: this.$t("problem.column.reaction"),
                    textAlign: 'center',
                    widthCell: 120,
                    name: "Reaction",
                    sortType: this.$enums.sortType.blur,
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                }
            ],
            problems: problems,
        }
    },
    computed: {
    },
    created() {
        this.initStaticData();
    },
    mounted() {
    },
    methods: {
        initStaticData() {
        }
    }


}
</script>
<style scoped>
@import './problem.css';
</style>
