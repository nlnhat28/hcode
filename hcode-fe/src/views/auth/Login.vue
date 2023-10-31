<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ $t('auth.login') }}
        </div>
        <div class="auth__body">
            <div class="auth__redirect">
                {{ $t('auth.noAccountYet') }}
                <v-button
                    link
                    :label="$t('auth.createAccount')"
                    @click="$router.push($path.signup);"
                />
            </div>
            <div class="auth__form">
                <!-- Tên người dùng hoặc email -->
                <v-input-text
                    ref="refUsernameOrEmail"
                    icon="fa fa-user"
                    hasClear
                    isRequired
                    isFocused
                    :label="$t('auth.usernameOrEmail')"
                />
                <!-- Mật khẩu -->
                <v-password
                    v-model="instance.password"
                    ref="refPassword"
                    icon="fa fa-key"
                    isRequired
                    :label="$t('auth.password')"
                    :tooltip="$t('auth.username')"
                />
            </div>
            <v-button
                :label="$t('auth.login')"
                @click="onClickSave()"
            />
            <div class="auth__forgot-password">
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

    },
    mounted() {
        this.refs = [
            this.$refs['refPassword'],
            this.$refs['refUsernameOrEmail'],
        ]
    },
    methods: {
        /**
         * Click Quên mật khẩu
         */
        clickForgotPassword() {

        }
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
