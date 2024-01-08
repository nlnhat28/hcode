<template>
    <div class="form__body">
        <slot name:="body"></slot>
    </div>
</template>
<script>
export default {
    name: 'VFormBody'
}
</script>
<style scoped>
@import './form.css';
</style>