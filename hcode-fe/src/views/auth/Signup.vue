<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.createAccount') }}
        </div>
        <div class="auth__body">
            <div class="auth__subtitle">
                {{ $t('auth.alreadyHaveAccount') }}
                <v-button
                    link
                    :label="$t('auth.login')"
                    @click="$router.push($path.login);"
                />
            </div>
            <div class="auth__form">
                <!-- Loading -->
                <v-mask-loading v-if="isLoading" />
                <!-- Tên người dùng -->
                <v-input-text
                    v-model="instance.Username"
                    ref="refUsername"
                    icon="fa fa-user"
                    hasClear
                    isRequired
                    isFocused
                    noSpace
                    :maxLength="50"
                    :label="$t('auth.username')"
                />
                <!-- Mật khẩu -->
                <v-password
                    v-model="instance.Password"
                    ref="refPassword"
                    icon="fa fa-key"
                    isRequired
                    isShowTitle
                    :warn="$vld.password"
                    :maxLength="255"
                    :label="$t('auth.password')"
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
                <!-- Email -->
                <v-input-text
                    v-model="instance.Email"
                    ref="refEmail"
                    icon="fa fa-envelope"
                    hasClear
                    isRequired
                    :validate="$vld.email"
                    :maxLength="100"
                    :label="'Email'"
                />
                <v-button
                    style="margin-top: 8px"
                    :label="$t('auth.signup')"
                    @click="onClickSave()"
                />
            </div>
        </div>
    </div>
</template>
<script>
import BaseAuth from "./BaseAuth.vue";

export default {
    name: "Auth",
    extends: BaseAuth,
    data() {
        return {
        }
    },
    mounted() {
        this.refs = [
            this.$refs['refEmail'],
            this.$refs['refConfirmPassword'],
            this.$refs['refPassword'],
            this.$refs['refUsername'],
        ]
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
    methods: {
        initOnCreated() {
            // this.instance = this.$cf.cloneDeep(this.authStore.auth);
        },
        /**
         * Override base
         */
        beforeValidate() {
            let email = this.instance.Email;

            if (email) {
                this.instance.Email = email.includes("@") ? email : `${email}@gmail.com`;
            };
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
         * Click đăng ký
         */
        async funcOnSave() {
            this.instance.VerifyMode = this.authEnum.verifyMode.signup;
            await this.signup(this.instance);
        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async signup(data) {
            try {
                const response = await this.instanceService.signup(data);
                if (this.$cf.onSuccess(response)) {
                    this.instance.AccountId = response.Data;
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
        afterSaveSuccess() {
            const data = {
                AccountId: this.instance.AccountId,
                Username: this.instance.Username,
                Password: this.instance.Password,
                Email: this.instance.Email,
                VerifyCode: this.instance.VerifyCode
            };
            this.authStore.setAuth(data);
            this.authStore.setVerifyMode(this.authEnum.verifyMode.signup);
            this.$router.push(this.$path.verify);
        }
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
