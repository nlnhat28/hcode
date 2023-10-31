<template>
    <div class="layout1">
        <TheHeader />
        <router-view name="view1"></router-view>
    </div>
</template>
<script>
import TheHeader from './TheHeader.vue';

export default {
    name: 'TheLayout1',
    components: {
        TheHeader,
    }
}
</script>
<style scoped>
@import './layout1.css';
</style>