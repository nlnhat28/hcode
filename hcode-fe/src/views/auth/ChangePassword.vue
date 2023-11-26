<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.changePassword') }}
        </div>
        <div class="auth__body">
            <div class="auth__subtitle">
                <p>
                    {{ $t('auth.changePasswordSubtitle') }}
                    <span class="yellow-400">{{ instance.Username }}</span>
                </p>
            </div>
            <div class="auth__form">
                <!-- Loading -->
                <v-mask-loading v-if="isLoading" />
                <!-- Mật khẩu -->
                <v-password
                    v-model="instance.Password"
                    ref="refPassword"
                    icon="fa fa-key"
                    isRequired
                    isShowTitle
                    :warn="$vld.password"
                    :maxLength="255"
                    :label="$t('auth.newPassword')"
                />
                <!-- Xác nhận mật khẩu -->
                <v-password
                    v-model="instance.ConfirmPassword"
                    ref="refConfirmPassword"
                    icon="fa fa-lock"
                    isConfirm
                    isRequired
                    :maxLength="255"
                    :label="$t('auth.confirmPassword')"
                    :password="instance.Password"
                />
                <v-button
                    style="margin-top: 8px"
                    :label="$t('com.save')"
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
            </div>
        </div>
    </div>
</template>
<script>
import BaseAuth from "./BaseAuth.vue";

export default {
    name: "ChangePassword",
    extends: BaseAuth,
    data() {
        return {
            documentTitle: this.$t("auth.changePassword"),
        }
    },
    watch: {
        "instance.Password"() {
            this.validConfirmPassword();
        },
        "instance.ConfirmPassword"() {
            this.validConfirmPassword();
        },
    },
    computed: {
    },
    mounted() {
        this.refs = [
            this.$refs['refConfirmPassword'],
            this.$refs['refPassword'],
        ]
    },
    methods: {
        initOnCreated() {
            this.instance = this.$cf.cloneDeep(this.authStore.auth);
        },
        /**
         * Xử lý thêm validate
         */
        customValidate() {
            if (this.messageValidate == null) {
                if (this.instance.Password != this.instance.ConfirmPassword) {
                    this.messageValidate = this.$t("auth.invalidConfirmPassword");
                    this.refError = this.$refs["refConfirmPassword"];
                    this.$refs["refConfirmPassword"].setErrorMessage(this.messageValidate);
                };
            };
        },
        /**
         * Validate xác nhận mật khẩu
         */
        validConfirmPassword() {
            if (this.instance.Password == this.instance.ConfirmPassword && this.$refs["refConfirmPassword"]) {
                this.$refs["refConfirmPassword"].clearErrorMessage();
            }
        },
        /**
         * Click đăng nhập
         */
        async funcOnSave() {
            const data = {
                AccountId: this.instance.AccountId,
                Username: this.instance.Username,
                Password: this.instance.Password
            };
            await this.changePassword(data);
        },
        /**
         * Đăng nhập
         *
         * Author: nlnhat (02/07/2023)
         */
        async changePassword(data) {
            try {
                const response = await this.accountService.changePassword(data);
                if (this.$cf.onSuccess(response)) {
                    this.isSuccessResponseFlag = true;
                    this.messageOnToast = this.$t("auth.successfullyChangePassword");
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
        afterSaveSuccess() {
            this.$router.push(this.$path.login);
        },
    }
}
</script>
<style scoped>
@import './auth.css';
</style>
