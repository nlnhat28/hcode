<template>
    <div class="p-inputgroup flex-1">
        <span class="p-inputgroup-addon" v-if="$slots.left">
            <slot name="left"></slot>
        </span>
        <slot name="center"></slot>
        <span class="p-inputgroup-addon" v-if="$slots.right">
            <slot name="right"></slot>
        </span>
    </div>
</template>
<script setup>
</script>