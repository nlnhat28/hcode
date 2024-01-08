<template>
    <div class="search-result">
        {{ this.$resources['vn'].searchResult }} "<b>
            <pre class="search-result__key">{{ keySearch }}</pre>
        </b>"
    </div>
</template>
<script>
export default {
    name: 'VSearchResult',
    props: {
        /**
         * Key search
         */
        keySearch: {
            type: String
        }
    },
}
</script>
<style scoped>
@import "./search.css";
</style>