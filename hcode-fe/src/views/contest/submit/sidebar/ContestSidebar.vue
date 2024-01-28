<template>
    <div :class="[
        'contest-sidebar p-relative',
        { 'contest-sidebar--collapse': collapseContestSidebar }
    ]">
        <v-mask-loading v-if="isLoading"></v-mask-loading>
        <div :class="['contest-sidebar__body']">
            <div class="label-20-yellow-300 p-relative">
                {{ instance.ContestName }}
            </div>
            <div class="w-full flex-column row-gap-28 pt-20">
                <div class="flex-column row-gap-24">
                    <div
                        v-for="(info, index) in infoComputed"
                        :key="index"
                        :class="['contest-info flex w-full p-relative']"
                    >
                        <div
                            class="contest-info__field"
                            style="width: 100px"
                        >
                            {{ info.field }}:
                        </div>
                        <div class="flex-1 font-bold">
                            {{ info.value }}
                        </div>
                    </div>
                </div>
                <div class="flex-1 flex-column row-gap-16">
                    <div class="color-text flex-center">
                        {{ $t('contest.problems') }}
                    </div>
                    <div
                        class="contest-problem__list"
                        v-if="!$cf.isEmptyArray(instance.ContestProblems)"
                    >
                        <ContestProblemItem
                            v-for="(item, index) in instance.ContestProblems"
                            :key="item.ContestProblemId"
                            :index="index"
                            :contestProblem="item"
                            :isSelected="item.ProblemId==problemId"
                            @selected="selectedItem"
                        ></ContestProblemItem>
                    </div>
                </div>
            </div>
        </div>
        <div class="contest-sidebar__footer">
            <v-button-container
                class="w-full"
                direction="row-reverse"
                justifyContent="space-between"
            >
                <!-- Nộp -->
                <v-button
                    :label="$t('contest.finish')"
                    @click="onClick(finishContest)"
                />
                <!-- Reload -->
                <v-button
                    icon="far fa-arrows-rotate"
                    severity="secondary"
                    outlined
                    :loading="isLoading"
                    @click="reloadInstance()"
                />
            </v-button-container>
        </div>
    </div>
</template>
<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { contestService, languageService } from "@/services/services";
import { useLanguageStore, useContestStore } from "@/stores/stores";
import { mapStores, mapState } from 'pinia';
import contestEnum from "@/enums/contest-enum";
import contestConst from "@/consts/contest-const.js";
import enums from "@/enums/enums";
import { problemService } from "@/services/services.js";
import ContestProblemItem from "./ContestProblemItem.vue";

const formMode = enums.formMode;

export default {
    name: "ContestSidebar",
    extends: BaseForm,
    components: {
        ContestProblemItem,
    },
    emits: ['selected', 'afterLoad'],
    props: {
        problemId: {
            type: String,
            default: null
        }
    },
    data() {
        return {
            collapseContestSidebar: false,
            cfg: {
                formPath: this.$path.contest,
                callbackPath: this.$path.contests,
                entity: 'Contest',
                subSysName: this.$t('contest.contest'),
            },
            instanceService: contestService,
            contestConst: contestConst,
            problems: [],
            dateTimePattern: 'dd/mm/yyyy hh/mm',
        }
    },
    watch: {
    },
    mounted() {
    },
    computed: {
        infoComputed() {
            try {
                if (this.instance) {
                    const i = this.instance;
                    let infos = [
                        {
                            field: this.$t('contest.field.contestCode'),
                            value: i.ContestCode,
                        },
                        {
                            field: this.$t('contest.field.accountFullName'),
                            value: i.AccountFullName,
                        },
                        {
                            field: this.$t('contest.field.start'),
                            value: this.$fm.formatDateTime(i.StartTime, this.dateTimePattern),
                        },
                        {
                            field: this.$t('contest.field.end'),
                            value: this.$fm.formatDateTime(i.EndTime, this.dateTimePattern),
                        },
                        {
                            field: this.$t('contest.field.time'),
                            value: i.TimeToDo ? `${i.TimeToDo} ${this.$t('com.minute')}` : this.$t('contest.noLimit'),
                        },
                    ]

                    return infos;
                }
            } catch (error) {
                console.error(error);
            }
            return [];
        },
    },
    methods: {
        /**
         * @override
         */
        async loadDataOnCreated() {
            await this.getForSubmit(this.instanceId);
        },
        /**
         * @override
         */
        customInstanceOnCreated() {
            if (this.instance) {
                this.instance.ContestAccount.StartTime = '2024-01-28T09:48:01+07:00';
            }
            if (!this.$cf.isEmptyArray(this.instance?.ContestProblems)
                && (!this.problemId || this.problemId == 0)) {
                this.selectedItem(this.instance.ContestProblems[0]);
            };
            this.$emit('afterLoad', this.instance);
        },
        /**
         * Lấy thông tin contest
         * @param {*} id 
         */
        async getForSubmit(id) {
            if (this.instanceService) {
                const response = await this.instanceService.getForSubmit(id);
                if (this.$res.isSuccess(response)) {
                    if (!this.$cf.isEmptyObject(response.Data)) {
                        this.instance = this.$cf.cloneDeep(response.Data);
                        this.storeOriginalInstance();

                        if (this.hasBuildDocumentTitle) {
                            this.documentTitle = this.instance[`${this.cfg.entity}Name`] + " - " + this.cfg.subSysName;
                            document.title = this.$cf.documentTitle(this.documentTitle);
                        };
                    }
                    else {
                        this.$dl.error(this.$t('msg.cannotFindRecord'))
                    }
                }
                else {
                    // this.handleError(response, this.goCallbackPath)
                    this.handleError(response)
                }
                this.processResponseGet(response);
            }
        },
        /**
         * Đóng mở contest sidebar
         */
        toggle() {
            this.collapseContestSidebar = !this.collapseContestSidebar;
        },
        /**
         * Chọn câu hỏi
         */
        selectedItem(item) {
            this.$emit('selected', item);
        },
        /**
         * Finish
         */
        async finishContest() {

        },
    }
}
</script>
<style scoped>
@import "./contest-sidebar.css";
</style>