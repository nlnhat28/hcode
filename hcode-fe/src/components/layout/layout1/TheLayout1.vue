<template>
    <div class="layout1">
        <div class="layout1__header">
            <TheHeader />
        </div>
        <div class="layout1__body">
            <router-view name="view1"></router-view>
        </div>
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