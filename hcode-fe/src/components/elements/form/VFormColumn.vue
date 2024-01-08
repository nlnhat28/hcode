<template>
    <div class="form__column" :style="`row-gap: ${rowGap}px;`">
        <slot></slot>
    </div>
</template>
<script>
export default {
    name: 'VFormColumn',
    props: {
        rowGap: {
            type: Number,
            default: 16
        }
    }
}
</script>
<style scoped>
@import './form.css';
</style>