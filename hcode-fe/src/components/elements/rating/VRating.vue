<template>
    <Rating>
        <template #onicon>
            <slot name="onicon"></slot>
        </template>
        <template #officon>
            <slot name="officon"></slot>
        </template>
    </Rating>
</template>
<script>
import Rating from 'primevue/rating';

export default {
    name: 'VRating',
    components: {
        Rating,
    }
}
</script>
<style>
@import './rating.css';
</style>