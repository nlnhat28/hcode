<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.login') }}
        </div>
        <div class="auth__body">
            <div class="auth__subtitle">
                {{ $t('auth.noAccountYet') }}
                <v-button
                    link
                    :label="$t('auth.createAccount')"
                    @click="$router.push($path.signup);"
                />
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
                <!-- Mật khẩu -->
                <v-password
                    v-model="instance.Password"
                    ref="refPassword"
                    icon="fa fa-key"
                    isRequired
                    :label="$t('auth.password')"
                    :tooltip="$t('auth.username')"
                />
                <v-button
                    style="margin-top: 8px"
                    :label="$t('auth.login')"
                    @click="onClickSave()"
                />
            </div>
            <div class="auth__footer">
                <v-button
                    link
                    :label="$t('auth.forgotPassword')"
                    @click="clickForgotPassword()"
                />
            </div>
        </div>
    </div>
</template>
<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { authService } from "@/services/services.js";
import { useAuthStore, useAccountStore } from "@/stores/stores.js";
import { mapStores, mapState } from 'pinia';
import authEnum from "@/enums/auth-enum.js"

export default {
    name: "Login",
    extends: BaseForm,
    data() {
        return {

        }
    },
    computed: {
        /**
         * Store
         */
        ...mapStores(useAuthStore),
        ...mapStores(useAccountStore),

    },
    mounted() {
        this.refs = [
            this.$refs['refPassword'],
            this.$refs['refUsername'],
        ]
    },
    methods: {
        initOnCreated() {
            this.mode = this.$enums.formMode.post;
            this.instanceService = authService;
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
            await this.login(data);
        },
        /**
         * Đăng nhập
         *
         * Author: nlnhat (02/07/2023)
         */
        async login(data) {
            try {
                const response = await this.instanceService.login(data);
                if (this.$cf.onSuccess(response)) {
                    this.isSuccessResponseFlag = true;
                    this.accountStore.setAccount(response.Data);
                    this.messageOnToast = this.$t("auth.successfullyLogin");
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
            this.$router.push(this.$path.problems);
        },
        /**
         * Click Quên mật khẩu
         */
        clickForgotPassword() {
            this.$router.push(this.$path.forgotPassword);
        },
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
