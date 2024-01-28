<template>
    <div class="contest-detail-container">
        <!-- Loading -->
        <v-mask-loading v-if="isLoading" />
        <div class="contest-detail__header">
            <div class="contest-detail__header--left flex-align-center">
                <v-button
                    text
                    severity="secondary"
                    icon="far fa-angle-left"
                    :label="$t('contest.contestList')"
                    @click="$router.push($path.contests)"
                ></v-button>
            </div>
            <div class="contest-detail__header--center flex-center">
                <div class="font-bold font-20 yellow-300">
                    {{ centerTitle }}
                </div>
            </div>
            <div class="contest-detail__header--right">

            </div>
        </div>
        <div class="contest-detail__body">
            <v-splitter>
                <v-splitter-panel class="flex-center">
                    <div class="contest-detail__info">
                        <v-tab-view>
                            <!-- Thông tin chung -->
                            <v-tab-panel :header="$t('contest.generalInfo')">
                                <v-form-body class="dark">
                                    <v-form-column :rowGap="32">
                                        <!-- Mã & Tên -->
                                        <v-form-group :columnGap="12">
                                            <!-- Mã -->
                                            <v-form-item
                                                style="width: 40%"
                                                :label="$t('contest.field.contestCode')"
                                            >
                                                <v-input-text
                                                    ref="refContestCode"
                                                    v-model="instance.ContestCode"
                                                    hasCopy
                                                    :maxLength="255"
                                                    :label="$t('contest.field.contestCode')"
                                                    :applyPlaceholder="false"
                                                >
                                                </v-input-text>
                                            </v-form-item>
                                            <!-- Tên -->
                                            <v-form-item
                                                isRequired
                                                :label="$t('contest.field.contestName')"
                                            >
                                                <v-input-text
                                                    ref="refContestName"
                                                    isRequired
                                                    v-model="instance.ContestName"
                                                    hasClear
                                                    :maxLength="255"
                                                    :label="$t('contest.field.contestName')"
                                                    :applyPlaceholder="false"
                                                >
                                                </v-input-text>
                                            </v-form-item>
                                        </v-form-group>
                                        <!-- Mật khẩu & Thời gian làm -->
                                        <v-form-group :columnGap="12">
                                            <!-- Mật khẩu -->
                                            <v-form-item
                                                style="width: 50%"
                                                :label="$t('contest.field.password')"
                                            >
                                                <v-password
                                                    ref="refPassword"
                                                    v-model="instance.Password"
                                                    hasClear
                                                    :maxLength="255"
                                                    :label="$t('contest.field.password')"
                                                    :applyPlaceholder="false"
                                                >
                                                </v-password>
                                            </v-form-item>
                                            <!-- Thời gian làm -->
                                            <v-form-item
                                                style="width: 50%"
                                                :label="$t('contest.field.timeToDoWithUnit')"
                                            >
                                                <v-input-text
                                                    ref="refTimeToDo"
                                                    v-model="instance.TimeToDo"
                                                    type='number'
                                                    :dataInput="$enums.dataInput.integer"
                                                    :minValue="1"
                                                    :validate="validateTimeToDo"
                                                    :label="$t('contest.field.timeToDoWithUnit')"
                                                    :applyPlaceholder="false"
                                                >
                                                </v-input-text>
                                            </v-form-item>
                                        </v-form-group>
                                        <!-- Thời gian bắt đầu & Thời gian kết thúc -->
                                        <v-form-group :columnGap="12">
                                            <!-- Thời gian bắt đầu -->
                                            <v-form-item
                                                class="dark"
                                                style="width: 50%"
                                                :label="$t('contest.field.startTime')"
                                            >
                                                <v-calendar
                                                    v-model="instance.StartTime"
                                                    showTime
                                                    hourFormat="24"
                                                    :dateFormat="dateTimeFormat"
                                                />
                                            </v-form-item>
                                            <!-- Thời gian kết thúc -->
                                            <v-form-item
                                                class="dark"
                                                style="width: 50%"
                                                :label="$t('contest.field.endTime')"
                                            >
                                                <v-calendar
                                                    v-model="instance.EndTime"
                                                    showTime
                                                    hourFormat="24"
                                                    :dateFormat="dateTimeFormat"
                                                />
                                            </v-form-item>
                                        </v-form-group>
                                        <div class="flex col-gap-24">
                                            <div class="checkbox-item">
                                                <!-- Trạng thái lưu -->
                                                <div class="label cursor-pointer">
                                                    {{ $t('contest.field.public') }}
                                                </div>
                                                <v-checkbox
                                                    v-model="instance.IsPublic"
                                                    binary
                                                >
                                                </v-checkbox>
                                            </div>
                                        </div>
                                    </v-form-column>
                                </v-form-body>
                            </v-tab-panel>
                            <!-- Danh sách tham gia -->
                            <v-tab-panel :header="$t('contest.listParticipants')">
                                <ContestAccountList />
                            </v-tab-panel>
                        </v-tab-view>
                    </div>
                </v-splitter-panel>
                <!-- Danh sách câu hỏi -->
                <v-splitter-panel class="flex-center">
                    <div class="contest-problem-container">
                        <div class="font-bold color-text flex-center">
                            {{ $t('contest.problems') }}
                        </div>
                        <v-button-container class="w-full justify-between">
                            <v-button
                                icon="far fa-plus"
                                :label="$t('com.add')"
                                @click="clickAddContestProblem"
                            ></v-button>
                            <v-button
                                v-if="!$cf.isEmptyArray(instance.ContestProblems)"
                                severity="danger"
                                icon="far fa-trash-can"
                                outlined
                                :label="$t('com.deleteAll')"
                                @click="clickDeleteAllContestProblem"
                            ></v-button>
                        </v-button-container>
                        <div
                            class="contest-problem__list"
                            v-if="!$cf.isEmptyArray(instance.ContestProblems)"
                        >
                            <ContestProblemItem
                                v-for="(problem, index) in instance.ContestProblems"
                                :key="problem.ContestProblemId"
                                :index="index"
                                :contestProblem="problem"
                                :contestProblems="instance.ContestProblems"
                                :problems="problems"
                                @onDelete="deleteContestProblem"
                            ></ContestProblemItem>
                        </div>
                        <div
                            class="flex-align-center justify-between"
                            v-if="!$cf.isEmptyArray(instance.ContestProblems)"
                        >
                            <div class="color-text flex-center col-gap-8">
                                <span>
                                    {{ $t('contest.totalProblem') }}:
                                </span>
                                <b class="color-highlight"> {{ $cf.getCount(instance.ContestProblems) }}</b>
                            </div>
                            <div class="color-text flex-center col-gap-8">
                                <span>
                                    {{ $t('contest.totalScore') }}:
                                </span>
                                <b class="color-highlight"> {{ totalScore }}</b>
                            </div>
                        </div>
                    </div>
                </v-splitter-panel>
            </v-splitter>
        </div>
        <div class="contest-detail__footer">
            <v-button-container direction="row-reverse">
                <!-- Đóng -->
                <v-button
                    v-if="showExit"
                    :label="$t('com.close')"
                    @click="$router.push($path.contests)"
                />
                <!-- Lưu -->
                <v-button
                    v-else
                    :label="$t('com.save')"
                    @click="onClickSave"
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
import ContestProblemItem from "./ContestProblemItem.vue";
import enums from "@/enums/enums";
import { problemService } from "@/services/services.js";
import ContestAccountList from "./contest-account/ContestAccountList.vue";

const formMode = enums.formMode;

export default {
    name: "ContestDetail",
    extends: BaseForm,
    components: {
        ContestProblemItem,
        ContestAccountList
    },
    data() {
        return {
            cfg: {
                formPath: this.$path.contest,
                callbackPath: this.$path.contests,
                entity: 'Contest',
                subSysName: this.$t('contest.contest'),
            },
            instanceService: contestService,
            contestConst: contestConst,
            problems: [],
            dateTimeFormat: 'dd/mm/yy',
            showExit: false,
        }
    },
    watch: {
        "instance.StartTime"() {
            this.$cf.checkValidateRef(this.$refs.refTimeToDo);
        },
        "instance.EndTime"() {
            this.$cf.checkValidateRef(this.$refs.refTimeToDo);
        },

    },
    mounted() {
        this.refs = [
            this.$refs.refTimeToDo,
            this.$refs.refContestName,
            this.$refs.refContestCode,
        ]
    },
    computed: {
        ...mapStores(useContestStore),
        /**
         * Tạo tiêu đề contest
         */
        centerTitle() {
            let title = '';
            // if (!this.$cf.isEmptyString(this.instance.ContestCode)) {
            //     title = `${this.instance.ContestCode}. `;
            // };
            if (!this.$cf.isEmptyString(this.instance.ContestName)) {
                title += this.instance.ContestName;
            }

            return title;
        },
        /**
         * Tổng điểm
         */
        totalScore() {
            return this.$cf.calcTotal(this.instance.ContestProblems, 'Score')
        }
    },
    methods: {
        /**
         * Khởi tạo dữ liệu data
         */
        async initOnCreated() {

            let id = this.$route.params.id;

            if (id == null) {
                this.mode = formMode.create;
                this.instance = this.contestStore.contest;
            }

        },
        /**
         * Khởi tạo contest khi thêm mới
         * @override
         */
        initCreateInstance() {
            this.documentTitle = this.$t("contest.createContest");
            document.title = this.$cf.documentTitle(this.documentTitle);
        },
        /**
         * Lấy danh sách problem
         */
        async customLoadDataOnCreated() {
            const res = await problemService.getForContest();
            if (this.$res.isSuccess(res)) {
                this.problems = res.Data;
            };
        },
        /**
         * @override
         */
        async customInstanceOnCreated() {
            if (this.instance) {
                if (this.instance.StartTime) {
                    this.instance.StartTime = new Date(this.instance.StartTime);
                }
                if (this.instance.EndTime) {
                    this.instance.EndTime = new Date(this.instance.EndTime);
                }
            }
        },
        /**
         * Thêm problem
         */
        clickAddContestProblem() {
            if (!this.instance.ContestProblems) {
                this.instance.ContestProblems = []
            };

            const problem = {
                ContestProblemId: this.$cf.uuid.new(),
            }

            this.instance.ContestProblems.push(problem);
        },
        /**
         * Xoá 1 problem
         * 
         * @param {*} problem 
         */
        deleteContestProblem(problem) {
            this.instance.ContestProblems = this.instance.ContestProblems.filter(item => item.ContestProblemId != problem.ContestProblemId)
        },
        /**
         * Click xoá problem
         */
        clickDeleteAllContestProblem() {
            const header = this.$t("contest.deleteAllProblem");
            const message = this.$t('contest.deleteAllProblemConfirm');
            const buttons = [
                {
                    // Huỷ
                    severity: "secondary",
                    outlined: true,
                    label: this.$t("com.cancel"),
                    icon: "fa fa-xmark",
                },
                {
                    // Xoá
                    severity: "danger",
                    label: this.$t("com.delete"),
                    icon: "far fa-trash-can",
                    autofocus: true,
                    click: this.deleteAllContestProblem,
                }
            ];
            this.$dl.confirm(message, buttons, header);
        },
        /**
         * Xoá hết contest problem
         */
        deleteAllContestProblem() {
            this.instance.ContestProblems = [];
        },
        /**
         * Trước khi doSave()
         * @virtual
         */
        beforeDoSave() {
        },
        /**
         * Click lưu nháp
         */
        onClickSaveDraft() {
            try {
                console.log("Saving...:", this.instance);
                this.doSave();
            } catch (error) {
                console.error(error);
            }
            // this.onClickSave();
        },
        /**
         * Xử lý thêm validate tại đây
         */
        customValidate() {
            // Validate testcase
            if (this.messageValidate == null) {
                if (this.instance.StartTime && this.instance.EndTime) {
                    if (this.instance.StartTime >= this.instance.EndTime) {
                        this.messageValidate = this.$t("contest.endTimeMoreThanStartTime");
                        return;
                    }
                }
                if (this.instance.ContestProblems && this.instance.ContestProblems.length > 0) {
                    let invalidQuestions = this.instance.ContestProblems
                        .filter(item => !item.ProblemId)
                        .map(item => item.Order);

                    if (!this.$cf.isEmptyArray(invalidQuestions)) {
                        this.messageValidate = this.$t("contest.invalidProblemInQuestion", {
                            question: invalidQuestions.join(", ")
                        });
                    }
                }
                else {
                    this.messageValidate = this.$t("contest.addAtLeastProblem");
                }
            };
        },
        /**
         * Validate thời gian để làm
         */
        validateTimeToDo(label, value) {
            if (this.instance.StartTime && this.instance.EndTime) {
                let limit = this.$cf.getMinuteTwoDateTime(this.instance.StartTime, this.instance.EndTime);

                if (value > 0 && limit > 0 && value > limit) {
                    return this.$t("contest.timeToDoNotMoreLimit", { limit: limit })
                }
            };
            return null;
        },
        /**
         * Focus vào tab view
         * 
         * @param {*} tabViewIndex Index tab
         */
        focusTabView(tabViewIndex) {
            this.activeTab = tabViewIndex;
        },
        /**
         * Xử lý response createInstance
         * @override
         */
        processResponseCreate(response) {
            if (this.$res.isSuccess(response)) {
                if (response.Data && response.Data.ContestCode) {
                    this.instance.ContestCode = response.Data.ContestCode;
                }
            }
        },
        /**
         * Xử lý response updateInstance
         * @override
         */
        processResponseUpdate(response) {
            if (this.$res.isSuccess(response)) {
                if (response.Data && response.Data.ContestCode) {
                    this.instance.ContestCode = response.Data.ContestCode;
                }
            }
        },
        /**
         * Lưu thành công
         */
        afterSaveSuccess() {
            this.$ts.success(this.messageOnToast);
            this.showExit = true;
        },
    }
}
</script>
<style scoped>
@import './contest-detail.css';
</style>