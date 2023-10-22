<template>
    <div class="home-container">
        <v-logo-hcode full/>
        <div class="home__content">
            <div class="home__content--left">
                <div class="home__title">
                    <div class="home__big-title">
                        {{ $t("home.bigTitle") }}
                    </div>
                    <div class="home__small-title">
                        {{ $t('home.smallTitle') }}
                    </div>
                    <v-button
                        :label="$t('home.signup')"
                        @click="clickCreateAccount"
                    ></v-button>
                </div>
            </div>
            <div class="home__content--right">
            </div>
        </div>
    </div>
</template>
<script>

export default {
    name: "Home",
    data() {
        return {
        }
    },
    methods: {
        /**
         * Click tạo tài khoản
         */
        clickCreateAccount() {
            this.$router.push(this.$path.signup);
        }
    }
}
</script>
<style scoped>
.home-container {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100vh;
    padding: 60px 120px;
    overflow: hidden;
    background: url(@/assets/img/bg-home.jpg) no-repeat;
    background-size: 100% 100%;
}

.home__content {
    display: flex;
    width: 100%;
    height: 100%;
}

.home__content--left {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: flex-start;
    row-gap: 24px;
    width: 50%;
    height: 100%;
}

.home__content--right {
    display: flex;
    align-items: center;
    width: 50%;
    height: 100%;
}

.home__title {
    display: flex;
    justify-content: center;
    align-items: flex-start;
    flex-direction: column;
    row-gap: 24px;
    width: 80%;
    height: fit-content;
    color: #fff;
}

.home__big-title {
    font-size: 60px;
    font-weight: bolder;
    font-family: "Consolas"
}

.home__small-title {
    font-family: "Consolas";
    font-size: 20px;
    color: var(--grey-400);
    line-height: 1.2;
}
</style>
