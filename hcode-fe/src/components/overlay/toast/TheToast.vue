<template>
    <Toast class="v-toast" />
</template>

<script>
import Toast from "primevue/toast";

export default {
    name: "TheToast",
    components: {
        Toast,
    },
    created() {
        this.$emitter.on("toastSuccess", (message) => {
            this.addToast("success", message);
        });
        this.$emitter.on("toastError", (message) => {
            this.addToast("error", message);
        });
        this.$emitter.on("toastWarn", (message) => {
            this.addToast("warn", message);

        });
        this.$emitter.on("toastInfo", (message) => {
            this.addToast("info", message);
        });
    },
    unmounted() {
        this.$emitter.off("toastSuccess");
        this.$emitter.off("toastError");
        this.$emitter.off("toastWarn");
        this.$emitter.off("toastInfo");
    },
    methods: {
        /**
         * Hàm add toast
         */
        addToast(severity, message) {
            this.$toast.add({
                severity: severity,
                summary: this.$t(`com.${severity}`),
                detail: message,
                life: window.config.toast.life,
            });
        }
    }
};
</script>
<style>
@import "./toast.css";
</style>