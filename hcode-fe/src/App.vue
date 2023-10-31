<template>
  <div class="app-container">
    <router-view
      name="viewApp"
      v-if="$route.matched.some(route => route.components.viewApp)"
    ></router-view>
    <TheLayout1 v-if="!$route.matched.some(route => route.components.viewApp)" />
    <TheDialog />
    <TheToast />
  </div>
</template>

<script>
import TheLayout1 from "@/components/layout/layout1/TheLayout1.vue"
import TheDialog from "@/components/overlay/dialog/TheDialog.vue"
import TheToast from "@/components/overlay/toast/TheToast.vue"

export default {
  name: 'App',
  components: {
    TheLayout1,
    TheDialog,
    TheToast
  },
  created() {
    document.title = window.config.app.title;
  }
}
</script>

<style scoped>
.app-container {
  width: 100%;
  height: 100%;
}</style>
