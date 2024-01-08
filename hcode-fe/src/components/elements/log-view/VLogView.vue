<template>
    <div class="v-log-view">
        <div
            class="log-view__title"
            v-if="!$cf.isEmptyString(title)"
        >
            {{ title }}
        </div>
        <!-- <div v-if="!$cf.isEmptyString(title)"></div> -->
        <div class="log-view__content">
            {{ content }}
        </div>
    </div>
</template>
<script>
export default {
    name: 'VLogView',
    props: {
        /**
         * Tiêu đề
         */
        title: {
            type: [String, Number, Object, Array],
            default: null,
        },
        /**
         * Nội dung
         */
        content: {
            type: [String, Number, Object, Array],
            default: null,
        }
    }
}
</script>
<style scoped>@import "./log-view.css";</style>