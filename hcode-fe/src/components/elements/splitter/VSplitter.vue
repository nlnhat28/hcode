<template>
    <Splitter>
        <template #default>
            <slot name="default"></slot>
        </template>
    </Splitter>
</template>
<script>
import Splitter from "primevue/splitter";

export default {
    name: 'VSplitter',
    components: {
        Splitter,
    }
}
</script>
<style scoped>
@import './spiltter.css';
</style>