<template>
    <div class="auth-container">
    </div>
</template>
<script>
import BaseForm from "@/components/base/BaseForm.vue";

export default {
    name: "ProblemsList",
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
@import './problem.css';
</style>
