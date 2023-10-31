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

            </div>
        </div>
    </div>
</template>
<script>
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
                    to: this.$path.login
                },
                {
                    id: 2,
                    text: this.$t("com.about"),
                    to: this.$path.signup
                }
            ]
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
    background-color: rgba(0, 0, 0, 0.274);
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