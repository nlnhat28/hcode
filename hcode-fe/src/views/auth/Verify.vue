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
    mounted() {
        this.refs = [
            this.$refs['refVerifyCode'],
        ]
    },
    computed: {
        /**
         * Store
         */
        ...mapStores(useAuthStore),
    },
    methods: {
        initOnCreated() {
            this.mode = this.$enums.formMode.post;
            this.instanceService = authService;
            this.instance = this.authStore.auth;
            this.instance.VerifyCode = null;
        },
        /**
         * Click xác thực
         */
        async funcOnSave() {
            const data = {
                Username: this.instance.Username,
                VerifyCode: this.instance.VerifyCode
            };
            await this.verify(data);
        },
        /**
         * Gửi lại mã
         */
        async clickSendVerifyCode() {
            this.disabledSendVerifyCode = true;
            this.sendVerifyCode(this.instance);
            await this.$cf.sleep(5);
            this.disabledSendVerifyCode = false;
        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async verify(data) {
            try {
                const response = await this.instanceService.verify(data);
                if (response?.status == this.$enums.httpStatus.ok) {
                    this.isSuccessResponseFlag = true;
                    this.messageOnToast = this.$t("auth.verifedEmail");
                } else {
                    this.isSuccessResponseFlag = false;
                }
            } catch (error) {
                console.error(error);
                this.isSuccessResponseFlag = false;
            }
        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async sendVerifyCode(data) {
            try {
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
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
