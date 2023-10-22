<template>
    <div class="logo-container">
        <img
            class="logo--hcode"
            alt="logo-hcode"
            :src="src"
        />
    </div>
</template>

<script>
export default {
    name: "LogoHCode",
    props: {
        /**
         * Logo full text hay chỉ logo, mặc định chỉ logo
         */
        full: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            src: this.full ? 'src/assets/img/logo-text.svg' : 'src/assets/img/logo-h.svg',
        }
    }
}
</script>

<style scoped>
@import './logo.css';
</style>