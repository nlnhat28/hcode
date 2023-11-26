<template>
    <div class="auth-container">
        <v-mask-dialog
            v-if="isLoging"
            type="spinner"
            :text="$t('auth.loging')"
        />
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.verifyEmail') }}
        </div>
        <div class="auth__body">
            <div class="auth__subtitle">
                <p>
                    {{ $t('auth.verifySubtitle') }}
                    <span class="yellow-400">{{ instance.Email }}</span>.
                    {{ $t('auth.typeVerifyCode') }}
                </p>
            </div>
            <div class="auth__form">
                <v-mask-loading v-if="isLoading" />
                <!-- Tên người dùng -->
                <v-input-text
                    v-model="instance.VerifyCode"
                    ref="refVerifyCode"
                    icon="fa fa-badge-check"
                    hasClear
                    isRequired
                    isFocused
                    :maxLength="6"
                    :label="$t('auth.verifyCode')"
                />
                <v-button
                    style="margin-top: 8px"
                    :label="$t('auth.verify')"
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
                    :loading="sendCodeState.isLoading"
                    :disabled="sendCodeState.isDisabled"
                    :label="$t('auth.sendVerifyCodeAgain')"
                    :icon="sendCodeState.isSuccess ? 'fa fa-check' : null"
                    @click="clickSendVerifyCode()"
                />
            </div>
        </div>
    </div>
</template>
<script>
import BaseAuth from "./BaseAuth.vue";

export default {
    name: "Verify",
    extends: BaseAuth,
    data() {
        return {
            documentTitle: this.$t("auth.verifyEmail"),
            /**
             * Đang đăng nhập
             */
            isLoging: false,
            /**
             * Gửi mã thành công
             */
            sendCodeState: {
                isSuccess: false,
                isDisabled: false,
                isLoading: false,
            },
        }
    },
    mounted() {
        this.refs = [
            this.$refs['refVerifyCode'],
        ]
    },
    computed: {
    },
    methods: {
        initOnCreated() {
            this.instance = this.$cf.cloneDeep(this.authStore.auth);
            this.instance.VerifyCode = null;
        },
        /**
         * Click xác thực
         */
        async funcOnSave() {
            this.sendCodeState.isDisabled = false;
            const data = {
                Username: this.instance.Username,
                VerifyCode: this.instance.VerifyCode
            };
            await this.verify(data);
        },
        /**
         * Verify
         *
         * Author: nlnhat (02/07/2023)
         */
        async verify(data) {
            try {
                const response = await this.instanceService.verify(data);
                if (response && response?.Success) {
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
         * Xử lý khi lưu thành công
         */
        async afterSaveSuccess() {
            switch (this.authStore.auth.verifyMode) {
                // Là form đăng ký thì tiến hành đăng nhập
                case (this.authEnum.verifyMode.signup):
                    let res = false;
                    try {
                        this.isLoging = true;
                        res = await this.login();
                    } finally {
                        this.isLoging = false;
                    }

                    if (res) {
                        this.$router.push(this.$path.problems);
                    };

                    break;
                // Là form đổi mật khẩu
                case (this.authEnum.verifyMode.changePassword):
                    this.$router.push(this.$path.changePassword);
                    break;
                default:
                    break;
            }
        },
        /**
         * Đăng nhập sau khi xác thực
         */
        async login() {
            const data = {
                AccountId: this.instance.AccountId,
                Username: this.instance.Username,
                Password: this.instance.Password
            };
            const response = await this.accountService.login(data);
            if (this.$cf.onSuccess(response)) {
                return true;
            } else {
                this.handleError(response);
                return false;
            };
        },
        /**
         * Gửi lại mã
         */
        async clickSendVerifyCode() {
            this.sendCodeState.isDisabled = true;
            this.sendCodeState.isLoading = true;

            const data = {
                Username: this.instance.Username,
                Email: this.instance.Email,
                VerifyMode: this.instance.VerifyMode
            };
            const response = await this.instanceService.sendVerifyCode(data);
            if (this.$cf.onSuccess(response)) {
                this.sendCodeState.isSuccess = true;
            }
            else {
                handleError(response);
            };

            this.sendCodeState.isLoading = false;

            await this.$cf.sleep(5);

            this.sendCodeState.isSuccess = false;
        },
        /**
         * Validate lại email
         */
        customValidate() {
            if (this.messageValidate == null) {
                if (!this.$reg.email(this.instance.Email))
                    this.messageValidate = this.$t("msg.invalidEmail");
            };
        },
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
