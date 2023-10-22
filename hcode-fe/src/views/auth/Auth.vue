<template>
    <div class="auth-container">
        <v-logo-hcode />
        <div class="auth__title">
            {{ titleComputed }}
        </div>
        <div class="auth_body">
            <div class="auth__form">
                <!-- Tên người dùng -->
                <v-input-text
                    ref="refUsername"
                    icon="fa fa-user"
                    hasClear
                    isRequired
                    isFocused
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
                <v-button
                    :label="$t('auth.signup')"
                    @click="onClickSave()"
                ></v-button>
            </div>
        </div>
    </div>
</template>
<script>
import authEnum from "@/enums/auth-enum.js";
import BaseForm from "@/components/base/BaseForm.vue";

export default {
    name: "Auth",
    extends: BaseForm,
    props: {
        /**
         * Kiểu form
         */
        formType: {
            type: Number,
            default: authEnum.formType.createAccount,
            validator: (value) => {
                return [
                    authEnum.formType.login,
                    authEnum.formType.createAccount,
                ].includes(value);
            },
        }
    },
    computed: {
        /**
         * Tiêu đề 
         */
        titleComputed() {
            switch (this.formType) {
                case authEnum.formType.login:
                    return this.$t('auth.login.')
                case authEnum.formType.createAccount:
                    return this.$t('auth.createAccount')
            }
        }
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
.auth-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    row-gap: 24px;
    width: 100%;
    height: 100vh;
    padding: 40px 120px;
    overflow: hidden;
    background: url(@/assets/img/bg-auth.jpg) no-repeat;
    background-size: 100% 100%;
}

.auth__title {
    font-size: 48px;
    font-weight: bolder;
    font-family: "Consolas";
    color: #fff;
    margin-bottom: 24px;
}

.auth_body {
    display: flex;
    flex-direction: column;
    row-gap: 24px;
}

.auth__form {
    width: 320px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    row-gap: 16px;
}
</style>
