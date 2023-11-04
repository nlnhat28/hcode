<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.verifyEmail') }}
        </div>
        <div class="auth__body">
            <div class="auth__subtitle">
                <p>
                    {{ $t('auth.verifySubtitle') }}
                    <span class="yellow-400">{{ authStore.auth.Email }}</span>.
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
                    @click="$router.push($path.signup)"
                />
                <v-button
                    link
                    :loading="disabledSendVerifyCode"
                    :disabled="disabledSendVerifyCode"
                    :label="$t('auth.sendVerifyCodeAgain')"
                    @click="clickSendVerifyCode()"
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
    name: "Verify",
    extends: BaseForm,
    data() {
        return {
            instanceService: authService,
            /**
             * Disable nút gửi lại code
             */
            disabledSendVerifyCode: false,
        }
    },
    computed: {
    },
    created() {
        this.mode = this.$enums.formMode.post;
        this.instanceService = authService;
    },
    mounted() {
        this.refs = [
            this.$refs['refVerifyCode'],
        ]
    },
    watch: {
    },
    computed: {
        /**
         * Store
         */
        ...mapStores(useAuthStore),
    },
    methods: {
        /**
         * Gửi lại mã
         */
        async clickSendVerifyCode() {
            this.disabledSendVerifyCode = true;
            await new Promise((resolve) => setTimeout(resolve, 5000));
            this.disabledSendVerifyCode = false;
        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async sendVerifyCode(data) {
            try {
                // return this.$cf.sleep(10);
                const response = await this.instanceService.sendVerifyCode(data);
                if (response?.status == this.$enums.httpStatus.ok) {
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
            if (this.instance.Password == this.instance.ConfirmPassword) {
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
                // return this.$cf.sleep(10);
                const response = await this.instanceService.signup(data);
                if (response?.status == this.$enums.httpStatus.ok) {
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
        }
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
