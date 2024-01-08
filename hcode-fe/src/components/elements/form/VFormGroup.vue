<template>
    <div class="form__group" :style="`column-gap: ${columnGap}px;`">
        <slot></slot>
    </div>
</template>
<script>
export default {
    name: 'VFormGroup',
    props: {
        columnGap: {
            type: Number,
            default: 8
        },
    }
}
</script>
<style scoped>
@import './form.css';
</style>