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
                <v-mask-loading v-if="isLoading" />
                <!-- Tên người dùng -->
                <v-input-text
                    v-model="instance.username"
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
                    :warn="$vld.password"
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
                    v-model="instance.email"
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
    watch: {
        "instance.password"() {
            this.validConfirmPassword();
        },
        "instance.confirmPassword"() {
            this.validConfirmPassword();
        },
    },
    computed: {
        reformatInstance() {
        },
    },
    methods: {
        /**
         * Override base
         */
        beforeValidate() {
            let email = this.instance.email;

            if (email) {
                this.instance.email = email.includes("@") ? email : `${email}@gmail.com`;
            };
        },
        /**
         * Xử lý thêm validate
         */
        customValidate() {
            if (this.messageValidate == null) {
                if (this.instance.password != this.instance.confirmPassword) {
                    this.messageValidate = this.$t("auth.invalidConfirmPassword");
                    this.refError = this.$refs["refConfirmPassword"];
                    this.$refs["refConfirmPassword"].setErrorMessage(this.messageValidate);
                };
            };
        },
        /**
         * 
         */
        validConfirmPassword() {
            if (this.instance.password == this.instance.confirmPassword) {
                this.$refs["refConfirmPassword"].clearErrorMessage();
            }
        },
        /**
         * Click đăng ký
         */
        async funcOnSave() {
            const salt = this.$cf.genSalt();
            const password = this.$cf.hash(this.instance.password, salt);

            let data = {
                username: this.instance.username,
                email: this.instance.email,
                password: password,
                salt: salt,
            };

            await this.signup(data);
        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async signup(data) {
            try {
                return this.$cf.sleep(10);
                const response = await instanceService.signup(data);
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
