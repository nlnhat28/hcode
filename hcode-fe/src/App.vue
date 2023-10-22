<template>
  <div class="app-container">
    <router-view></router-view>
    <TheDialog />
    <TheToast />
  </div>
</template>

<script setup>
import TheDialog from "@/components/overlay/dialog/TheDialog.vue"
import TheToast from "@/components/overlay/toast/TheToast.vue"
/**
 * Hook
 */
document.title = window.config.app.title;
</script>

<style scoped>
.app-container {
  width: 100%;
  height: 100%;
}
</style>
