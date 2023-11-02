<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.createAccount') }}
        </div>
        <div class="auth__body">
            <div class="auth__redirect">
                {{ $t('auth.alreadyHaveAccount') }}
                <v-button
                    link
                    :label="$t('auth.login')"
                    @click="$router.push($path.login);"
                />
            </div>
            <div class="auth__form">
                <!-- Tên người dùng -->
                <v-input-text
                    ref="refUsername"
                    icon="fa fa-user"
                    hasClear
                    isRequired
                    isFocused
                    :maxLength="50"
                    :label="$t('auth.username')"
                />
                <!-- Mật khẩu -->
                <v-password
                    v-model="instance.password"
                    ref="refPassword"
                    icon="fa fa-key"
                    isRequired
                    :validate="$vld.password"
                    :maxLength="255"
                    :label="$t('auth.password')"
                    :tooltip="$t('auth.username')"
                />
                <!-- Xác nhận mật khẩu -->
                <v-password
                    v-model="instance.confirmPassword"
                    ref="refConfirmPassword"
                    icon="fa fa-lock"
                    isConfirm
                    isRequired
                    :maxLength="255"
                    :label="$t('auth.confirmPassword')"
                    :password="instance.password"
                />
                <!-- Email -->
                <v-input-text
                    ref="refEmail"
                    icon="fa fa-envelope"
                    isRequired
                    :validate="$vld.email"
                    :maxLength="100"
                    :label="'Email'"
                />
            </div>
            <v-button
                :label="$t('auth.signup')"
                @click="clickSignup()"
            />
        </div>
    </div>
</template>
<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { authService } from "@/services/services.js"

export default {
    name: "Auth",
    extends: BaseForm,
    data() {
        return {

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
            this.$refs['refEmail'],
            this.$refs['refConfirmPassword'],
            this.$refs['refPassword'],
            this.$refs['refUsername'],
        ]
    },
    computed: {
        reformatInstance() {
            let data = {};

            return data;
        },
    },
    methods: {
        /**
         * Xử lý thêm validate
         */
        customValidate() {
            if (this.messageValidate == null) {
                if (this.instance.password != this.instance.confirmPassword) {
                    this.messageValidate = this.$t("auth.invalidConfirmPassword");
                    this.refError = this.$refs["refConfirmPassword"];
                    this.$refs["refConfirmPassword"].setErrorMessage(this.messageValidate);
                }
            }
        },
        /**
         * Click đăng ký
         */
        clickSignup() {

        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async signup() {
            try {
                const response = await instanceService.signup(this.reformatInstance);
                if (response?.status == this.$enums.httpstatus.created) {
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
