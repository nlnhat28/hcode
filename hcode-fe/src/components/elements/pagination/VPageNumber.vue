<template>
    <div
        class="pagination__page-number"
        :class="{ 'page-number--active': pageNumber.isActive }"
        @click="onActive()"
    >
        {{ pageNumber.number }}
    </div>
</template>
<script>
export default {
    name: 'VPageNumber',
    props: {
        pageNumber: {
            /**
             * Page number 
             */
            number: {
                type: Number,
            },
            /**
             * Is active or not
             */
            isActive: {
                type: Boolean,
                default: false
            }
        }
    },
    methods: {
        /**
         * Handle click this page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onActive() {
            this.$emit('emitPageActive')
        }
    }
}
</script>
<style scoped>
@import './pagination.css';
</style>