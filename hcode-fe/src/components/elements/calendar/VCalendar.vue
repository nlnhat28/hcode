<template>
    <Calendar class="v-calendar" />
</template>
<script>
import Calendar from "primevue/calendar";

export default {
    name: 'VCalendar',
    components: {
        Calendar,
    }
}
</script>
<style>
@import './calendar.css';
</style>