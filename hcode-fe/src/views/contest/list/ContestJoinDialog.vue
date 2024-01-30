<template>
    <v-dialog
        class="v-dialog"
        v-model:visible="isVisible"
        modal
        :header="headerComputed"
        :draggable="false"
        @hide="close"
        style="width: 600px;"
    >
        <template #closeIcon>
            <v-icon icon="far fa-circle-xmark" />
        </template>
        <div class="v-dialog__content">
            <div class="w-full flex-column row-gap-20 p-20">
                <div class="flex-column row-gap-20">
                    <div
                        class="contest-info flex w-full"
                        v-for="(info, index) in infoComputed"
                        :key="index"
                    >
                        <div
                            class="contest-info__field"
                            style="width: 160px"
                        >
                            {{ info.field }}:
                        </div>
                        <div class="flex-1 font-bold">
                            {{ info.value }}
                        </div>
                    </div>
                </div>
                <div v-if="instance.HasPassword && isShowJoinButton">
                    <!-- Mật khẩu -->
                    <v-form-item
                        style="width: 90%;"
                        :label="$t('contest.field.password')"
                        isRequired
                    >
                        <v-password
                            ref="refPassword"
                            v-model="password"
                            hasClear
                            isRequired
                            isFocused
                            :maxLength="255"
                            :label="$t('contest.field.password')"
                            :applyPlaceholder="false"
                        >
                        </v-password>
                    </v-form-item>
                </div>
            </div>
        </div>
        <template #footer>
            <v-mask-loading v-if="isLoading" />
            <v-button-container justifyContent="space-between">
                <!-- Các nút bên trái -->
                <div>
                    <!-- Rời -->
                    <v-button
                        v-if="isShowLeaveButton"
                        icon="far fa-right-from-bracket"
                        severity="danger"
                        :label="$t('contest.leaveContest')"
                        @click="loadingEffect(leaveContest)"
                    ></v-button>
                </div>
                <!-- Các nút bên phải -->
                <div class="flex col-gap-8">
                    <!-- Đóng -->
                    <v-button
                        v-if="!0"
                        outlined
                        :label="$t('com.cancel')"
                        @click="close"
                    ></v-button>
                    <!-- Tham gia -->
                    <v-button
                        v-if="isShowJoinButton"
                        icon="far fa-check"
                        autofocus
                        :label="$t('contest.join')"
                        @click="onClick(joinContest)"
                    ></v-button>
                    <!-- Bắt đầu -->
                    <v-button
                        v-if="isShowStartButton"
                        icon="fa fa-play"
                        autofocus
                        :label="$t('contest.start')"
                        @click="loadingEffect(startContest)"
                    ></v-button>
                    <!-- Tiếp tục bài thi -->
                    <v-button
                        v-if="isShowContinueButton"
                        icon="fa fa-play"
                        autofocus
                        :label="$t('contest.continue')"
                        @click="loadingEffect(continueContest)"
                    ></v-button>
                    <!-- Xem kết quả -->
                    <v-button
                        v-if="isShowResultButton"
                        icon="fa fa-table"
                        autofocus
                        :label="$t('contest.viewResult')"
                        @click="loadingEffect(resultContest)"
                    ></v-button>
                </div>
            </v-button-container>
        </template>
    </v-dialog>
</template>

<script>
import BaseFormDialog from '@/components/base/BaseFormDialog.vue';
import { contestService } from "@/services/services.js";
import contestEnum from "@/enums/contest-enum.js";

export default {
    name: "ContestJoinDialog",
    extends: BaseFormDialog,
    data() {
        return {
            instanceService: contestService,
            contestEnum: contestEnum,
            /**
             * Disable các button khi đang loading
             */
            disabledAllButton: false,
            password: null,
            dateTimePattern: 'dd/mm/yyyy hh/mm',
        }
    },
    computed: {
        headerComputed() {
            if (this.instance) {
                if (this.instance.State == contestEnum.contestState.finish.value) {
                    return this.$t('contest.contestFinished')
                }
                const caEnum = contestEnum.contestAccountState;
                switch (this.instance.ContestAccountState) {
                    case caEnum.pending.value:
                        return this.$t('contest.startContest')
                    case caEnum.doing.value:
                        return this.$t('contest.continueContest')
                    case caEnum.finish.value:
                        return this.$t('contest.contestAccountState.finished')
                    default:
                        return this.$t('contest.joinContest')
                }
            }
            return '';
        },
        infoComputed() {
            try {
                if (this.instance) {
                    const i = this.instance;
                    let infos = [
                        {
                            field: this.$t('contest.field.contestName'),
                            value: i.ContestName,
                        },
                        {
                            field: this.$t('contest.field.accountFullName'),
                            value: i.AccountFullName,
                        },
                        {
                            field: this.$t('contest.field.timeToDo'),
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
        /** Bài thi đã kết thúc */
        finishedContest() {
            if (this.instance) {
                return this.instance.State == contestEnum.contestState.finish.value;
            }
            return false;
        },
        /** Show nút tham gia */
        isShowJoinButton() {
            if (this.instance) {
                return this.instance.ContestAccountState == null
                    && !this.finishedContest;
            }
            return false;
        },
        /** Show nút tiếp tục bài thi */
        isShowContinueButton() {
            if (this.instance && this.instance.ContestAccountState) {
                return this.instance.ContestAccountState == contestEnum.contestAccountState.doing.value
                    && !this.finishedContest;
            }
            return false;
        },
        /** Show nút rời bài thi */
        isShowLeaveButton() {
            if (this.instance && this.instance.ContestAccountState) {
                return this.instance.ContestAccountState == contestEnum.contestAccountState.pending.value
                    && !this.finishedContest;
            }
            return false;
        },
        /** Show nút bắt đầu bài thi */
        isShowStartButton() {
            if (this.instance && this.instance.ContestAccountState) {
                return this.instance.ContestAccountState == contestEnum.contestAccountState.pending.value
                    && !this.finishedContest;
            }
            return false;
        },
        /** Show nút xem kết quả bài thi */
        isShowResultButton() {
            if (this.instance && this.instance.ContestAccountState) {
                return this.instance.ContestAccountState == contestEnum.contestAccountState.finish.value;
            }
            return false;
        },
    },
    mounted() {
        this.refs = [
            this.$refs.refPassword,
        ]
    },
    methods: {
        /**
         * @override
         */
        beforeValidate() {
            this.refs = [
                this.$refs.refPassword,
            ]
        },
        /**
         * Join
         */
        async joinContest() {
            if (this.instanceService) {
                const res = await this.instanceService.join({
                    ContestId: this.instance.ContestId,
                    Password: this.password,
                });

                if (this.$res.isSuccess(res)) {
                    this.reloadContest();
                    this.reload();
                    this.$ts.success(this.$t('contest.joinedContest'));
                }
                else {
                    this.handleError(res);
                }
            }
        },
        /**
         * Leave
         */
        async leaveContest() {
            if (this.instanceService && this.instance.ContestAccountState) {
                const res = await this.instanceService.leave(this.instance.ContestAccount.ContestAccountId);

                if (this.$res.isSuccess(res)) {
                    this.reloadContest();
                    this.reload();
                    this.$ts.success(this.$t('contest.leftContest'));
                }
                else {
                    this.handleError(res);
                }
            }
        },
        /**
         * Start
         */
        async startContest() {
            if (this.instanceService && this.instance.ContestAccountState) {
                const res = await this.instanceService.start(this.instance.ContestAccount.ContestAccountId);
                if (this.$res.isSuccess(res)) {
                    this.$ts.success(this.$t('contest.startContest'));
                    this.$router.push(this.$cf.combineRoute(
                        this.$path.contest, this.instance.ContestId, this.$path.submit, '0'));
                }
                else {
                    this.handleError(res);
                }
            }
        },
        /**
         * Tiếp tục
         */
        async continueContest() {
            if (this.instanceService && this.instance.ContestAccountState) {
                const res = await this.instanceService.continue(this.instance.ContestAccount.ContestAccountId);
                if (this.$res.isSuccess(res)) {
                    this.$router.push(this.$cf.combineRoute(
                        this.$path.contest, this.instance.ContestId, this.$path.submit, '0'));
                }
                else {
                    this.reload();
                    this.close();
                    this.handleError(res);
                }
            }
        },
        /**
         * Xem kết quả
         */
        async resultContest() {
            if (this.instanceService && this.instance.ContestAccountState) {
                this.$router.push(this.$cf.combineRoute(
                    this.$path.contestResult, this.instance.ContestId, this.$path.submit, '0'));

            }
        },
        /**
         * Load lại contest
         */
        async reloadContest() {
            const response = await this.instanceService.view(this.instanceId);
            if (this.$res.isSuccess(response)) {
                if (!this.$cf.isEmptyObject(response.Data)) {
                    this.instance = this.$cf.cloneDeep(response.Data);
                    this.storeOriginalInstance();
                }
                else {
                    this.close();
                }
            }
            else {
                this.close();
            }
        },
    }
};
</script>
<style scoped></style>