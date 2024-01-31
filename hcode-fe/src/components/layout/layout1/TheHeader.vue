<template>
    <div class="header">
        <div class="header__left">
            <div class="header__logo">
                <v-logo-hcode
                    isFull
                    size='sm'
                />
            </div>
        </div>
        <div class="header__center">
            <div class="header__nav">
                <v-nav :items="navItems" />
            </div>
        </div>
        <div class="header__right">
            <div class="header__toolbar">

            </div>
            <div class="header__auth">
                <div class="header__avatar">
                    <img
                        class="avatar--sm"
                        src="@/assets/img/default-avatar.png"
                        alt="Ảnh đại diện"
                    />
                </div>
                <div class="header__username">
                    {{ usernameComputed }}
                </div>
                <div class="header__dropdown">
                    <v-button
                        icon="pi pi-ellipsis-v"
                        @click="toggle"
                        aria-haspopup="true"
                        aria-controls="overlay_menu"
                    />
                    <v-menu
                        ref="menu"
                        id="overlay_menu"
                        :model="authMenu"
                        :popup="true"
                    />
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import { authService, accountService } from "@/services/services.js";
import { useAuthStore, useAccountStore } from "@/stores/stores.js";
import { mapStores } from 'pinia';
import authEnum from "@/enums/auth-enum.js";

export default {
    name: 'TheHeader',
    data() {
        return {
            /**
             * Item trong navbar
             */
            navItems: [
                {
                    id: 0,
                    text: this.$t("com.problems"),
                    to: this.$path.problems
                },
                {
                    id: 1,
                    text: this.$t("com.contests"),
                    to: this.$path.contests
                },
                {
                    id: 2,
                    text: this.$t("com.about"),
                    to: this.$path.signup
                }
            ],
            authMenu: [
                {
                    label: this.$t('auth.signup'),
                    icon: 'far fa-right-from-bracket'
                },
            ],
            /**
             * Auth
             */
            authService: authService,
            accountService: accountService,
            authEnum: authEnum,
        }
    },
    computed: {
        /**
         * Hiển thị tên người dùng
         */
        usernameComputed() {
            return this.accountStore.account.Username || this.accountStore.account.FullName;
        },
        /**
         * Store
         */
        ...mapStores(useAuthStore),
        ...mapStores(useAccountStore),
    },
    methods: {
        toggle(event) {
            this.$refs.menu.toggle(event);
        },
        logout() {
        }
    }
}
</script>
<style scoped>
.header {
    height: 72px;
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    column-gap: 16px;
    padding: 0 16px;
    background-color: var(--header-color);
    /* border-bottom: 1px solid #646464; */
}

.header__left,
.header__center,
.header__right {
    height: 100%;
    display: flex;
    align-items: center;
}

.header__center {
    flex: 1;
    justify-content: flex-start;
}

.header__left {
    width: 200px;
    justify-content: center;
}

.header__right {
    width: 460px;
    justify-content: flex-end;
    /* background-color: black; */
}

.header__auth {
    display: flex;
    align-items: center;
    column-gap: 12px;
    margin-right: 30px;
}

.header__username {
    color: #fff;
    font-family: Consolas;
    font-weight: 700;
    font-size: 18px;
}

.header__avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    /* border: 2px solid var(--grey-500); */
    display: flex;
    align-items: center;
    justify-content: center;
}

.header__avatar img.avatar--sm {
    width: 94%;
    height: 94%;
}

.header__logo {
    display: flex;
    align-items: center;
    justify-content: center;
    width: fit-content;
}

.header__nav {
    height: 100%;
}
</style>