<template>
    <div class="logo-container">
        <img
            alt="logo-hcode"
            :class="[
                'logo--hcode',
                `size--${size}`,
                { 'cursor-pointer': canClick }
            ]"
            :src="src"
            @click="clickLogo()"
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
        isFull: {
            type: Boolean,
            default: false,
        },
        /**
         * Click để về trang chủ
         */
        canClick: {
            type: Boolean,
            default: true,
        },
        /**
         * Size
         */
        size: {
            type: String,
            default: 'md',
            validator: (value) => {
                return [
                    'sm',
                    'md',
                    'lg',
                ].includes(value)
            }
        }
    },
    data() {
        return {
            src: this.isFull ? 'src/assets/img/logo-text.svg' : 'src/assets/img/logo-h.svg',
        }
    },
    methods: {
        /**
         * Click vào logo 
         */
        clickLogo() {
            if (this.canClick) {
                this.$router.push(this.$path.index);
            }
        }
    }
}
</script>

<style scoped>
@import './logo.css';
</style>