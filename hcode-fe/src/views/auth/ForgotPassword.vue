<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.forgotPassword') }}
        </div>
        <div class="auth__body">
            <div class="auth__subtitle">
                {{ $t('auth.typeOfUsername') }}
            </div>
            <div class="auth__form">
                <!-- Loading -->
                <v-mask-loading v-if="isLoading" />
                <!-- Tên người dùng -->
                <v-input-text
                    ref="refUsername"
                    v-model="instance.Username"
                    icon="fa fa-user"
                    hasClear
                    isRequired
                    isFocused
                    :label="$t('auth.username')"
                />
                <v-button
                    style="margin-top: 8px"
                    :label="$t('com.search')"
                    @click="onClickSave()"
                />
            </div>
            <div class="auth__footer auth__footer--verify">
                <v-button
                    style="color: #fff;"
                    link
                    icon="fa fa-angle-left"
                    :label="$t('com.back')"
                    @click="$router.back()"
                />
                <v-button
                    link
                    :label="$t('auth.createAccount')"
                    @click="this.$router.push(this.$path.signup)"
                />
            </div>
        </div>
    </div>
</template>
<script>
import BaseAuth from "./BaseAuth.vue";

export default {
    name: "ForgotPassword",
    extends: BaseAuth,
    data() {
        return {
            documentTitle: this.$t("auth.forgotPassword"),
        }
    },
    computed: {
    },
    mounted() {
        this.refs = [
            this.$refs['refUsername'],
        ]
    },
    methods: {
        initOnCreated() {
        },
        /**
         * Click đăng nhập
         */
        async funcOnSave() {
            const data = {
                Username: this.instance.Username
            };
            await this.forgotPassword(data);
        },
        /**
         * Đăng nhập
         *
         * Author: nlnhat (02/07/2023)
         */
        async forgotPassword(data) {
            try {
                const response = await this.instanceService.forgotPassword(data);
                if (this.$res.isSuccess(response)) {
                    this.instance.AccountId = response.Data.AccountId;
                    this.instance.Email = response.Data.Email;
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                    this.handleError(response);
                }
            } catch (error) {
                console.error(error);
                this.isSuccessResponseFlag = false;
            }
        },
        /**
         * Thành công
         */
        afterSaveSuccess() {
            this.showDialogEmail();
        },
        /**
         * Show dialog có email ứng với Username
         * 
         * @param {string} email
         */
        showDialogEmail() {
            const header = this.$t("auth.sendVerifyCode");
            const message = this.$t("auth.sendEmailToResetPassword", { email: this.instance.Email });
            const buttons = [
                {
                    // Huỷ
                    severity: "secondary",
                    outlined: true,
                    label: this.$t("com.cancel"),
                    icon: "fa fa-xmark",
                },
                {
                    // Gửi
                    label: this.$t("com.send"),
                    icon: "fa fa-paper-plane-top",
                    autofocus: true,
                    click: this.clickSendCode,
                }
            ];
            this.$dl.confirm(message, buttons, header);
        },
        /**
         * Click gửi mã -> Xác nhận email
         */
        async clickSendCode() {
            const data = {
                AccountId: this.instance.AccountId,
                Username: this.instance.Username,
                Email: this.instance.Email,
                VerifyMode: this.authEnum.verifyMode.changePassword,
            };
            const response = await this.instanceService.sendVerifyCode(data);
            if (this.$res.isSuccess(response)) {
                this.authStore.setAuth(data);
                this.authStore.setVerifyMode(this.authEnum.verifyMode.changePassword);
                this.$router.push(this.$path.verify);
            }
            else {
                handleError(response);
            };
        },

    }
}
</script>
<style scoped>
@import './auth.css';
</style>
