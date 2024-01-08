<template>
    <component
        :is="target ? 'router-link':'div'"
        :to="target"
        class="nav-item"
    >
        <div class="nav-item__icon">
            <v-icon :icon="icon"></v-icon>
            <span>
                <span
                    class="tooltip-wrapper"
                    v-show="title"
                >
                    <span
                        v-tooltip:right="title"
                        style="position: absolute; left: 12px;"
                    ></span>
                </span>
            </span>
        </div>
        <div class="menu-item__text">{{ title }}</div>
    </component>
</template>
<script>
export default {
    name: 'VNavItem',
    props: {
        /**
         * Target route
         */
        target: {
            type: String,
            default: null,
        },
        /**
         * Icon name
         */
        icon: {
            type: String
        },
        /**
         * Title of menu
         */
        title: {
            type: String
        }
    },
}
</script>
<style>
.sidebar--collapse .menu-item:hover [tooltip]::before,
.sidebar--collapse .menu-item:hover [tooltip]::after {
    display: block;
    opacity: 1;
    transition: opacity 0.6s;
}
</style>