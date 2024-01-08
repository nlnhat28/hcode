<template>
    <SplitterPanel>
        <template #default>
            <slot name="default"></slot>
        </template>
    </SplitterPanel>
</template>
<script>
import SplitterPanel from "primevue/splitterpanel";

export default {
    name: 'VSplitterPanel',
    components: {
        SplitterPanel,
    }
}
</script>
<style scoped>
@import './spiltter.css';
</style>