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
                    :label="$t('auth.confirmPassword')"
                    :password="instance.password"
                />
                <!-- Email -->
                <v-input-text
                    ref="refEmail"
                    icon="fa fa-envelope"
                    isRequired
                    :label="'Email'"
                />
            </div>
            <v-button
                :label="$t('auth.signup')"
                @click="onClickSave()"
            />
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
            this.$refs['refEmail'],
            this.$refs['refConfirmPassword'],
            this.$refs['refPassword'],
            this.$refs['refUsername'],
        ]
    }

}
</script>
<style scoped>
@import './auth.css';
</style>
