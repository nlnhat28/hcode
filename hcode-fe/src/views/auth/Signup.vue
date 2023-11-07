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
                    :warn="$vld.password"
                    :maxLength="255"
                    :label="$t('auth.password')"
                    :tooltip="$t('auth.username')"
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
import BaseForm from "@/components/base/BaseForm.vue";
import { authService } from "@/services/services.js";
import { useAuthStore } from "@/stores/stores.js";
import { mapStores, mapState } from 'pinia';

export default {
    name: "Auth",
    extends: BaseForm,
    data() {
        return {
            instanceService: authService,
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
        /**
         * Store
         */
        ...mapStores(useAuthStore),
    },
    methods: {
        initOnCreated()() {
            this.mode = this.$enums.formMode.post;
            this.instanceService = authService;
            this.instance = this.authStore.auth;
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
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async signup(data) {
            try {
                const response = await this.instanceService.signup(data);
                if (response?.status == this.$enums.httpStatus.ok ) {
                    this.instance.InstanceId = response.data.Data;
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                }
            } catch (error) {
                console.error(error);
                this.isSuccessResponseFlag = false;
            }
        },
        /**
         * Click đăng ký
         */
        async funcOnSave() {
            await this.signup(this.instance);
        },
        /**
         * Xử lý khi lưu thành công
         */
        afterSaveSuccess() {
            this.authStore.setAuth(this.instance);
            this.$router.push(this.$path.verify);
        }
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
