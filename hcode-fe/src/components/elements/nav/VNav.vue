<template>
    <div class="v-nav">
        <router-link
            v-for="(item, index) in items"
            :key=index
            :to="item.to"
            :class="[
                'v-nav-item',
            ]"
        >
            <div class="v-nav-item--active"></div>
            {{ item.text }}
        </router-link>
    </div>
</template>
<script>
export default {
    name: 'VNav',
    props: {
        /**
         * Các item trong nav. 
         * item: {
         *      id: Id, 
         *      to: Link, 
         *      text: Text hiển thị
         * }
         */
        items: {
            type: Array,
            default: []
        }
    }
}
</script>
<style scoped>
@import './nav.css';
</style>