<template>
    <div class="mask mask--no-content">
        <div class="mask--no-content__icon">
            <img class="mask--no-content__img" src="@/assets/img/no-content.png">
        </div>
        <div class="mask--no-content__message">
            {{ $t("msg.noContent") }}
        </div>
    </div>
</template>
<script>
export default {
    name: 'VNoContent'
}
</script>
<style scoped>
@import './mask.css';
</style>