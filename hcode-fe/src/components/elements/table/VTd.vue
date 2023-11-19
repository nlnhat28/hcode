<template>
    <td
        :style="`text-align: ${style.textAlign}; color: ${style.color}; font-weight: ${style.fontWeight}; `"
        :class="{
            'td--resizable': content,
        }"
        ref="td"
        @mouseover="onMouseOver"
    >
        <!-- default slot content -->
        <slot name="td"></slot>
        <span
            class="tooltip-wrapper"
            v-if="(isShowTooltip && content) || tooltip"
            :style="`width: ${widthTooltip}px`"
        >
            <!-- Trường hợp có tooltip -->
            <span
                v-if="tooltip"
                v-tooltip="tooltip"
                style="position: absolute; left: 50%"
            ></span>
            <!-- Trường hợp ko có tooltip thì dùng content -->
            <span
                v-else
                v-tooltip="content"
                style="position: absolute; left: 50%"
            ></span>
        </span>
        <div :class="[
            'td__content',
            { 'flex-start': style.textAlign == 'left' },
            { 'flex-center': style.textAlign == 'center' },
            { 'flex-start': style.textAlign == 'right' },
        ]">
            <div class="td__text">
                {{ content }}
            </div>
            <slot></slot>
        </div>
    </td>
</template>
<script>
export default {
    name: 'VTd',
    props: {
        /**
         * Style of cell
         */
        style: {
            type: Object,
            default: {
                textAlign: 'left',
                color: '#fff',
                fontWeight: 500,
            }
        },
        /**
         * Content of cell
         */
        content: {
            type: [String, Number],
            default: null
        },
        /**
         * Tooltip
         */
        tooltip: {
            type: [String, Number],
            default: null
        }
    },
    data() {
        return {
            /**
             * Show tooltip or not
             */
            isShowTooltip: false,
            /**
             * Width of tooltip
             */
            widthTooltip: 0,
        }
    },
    methods: {
        /**
         * Handle mouse over on td
         * 
         * Author: nlnhat (09/08/2023)
         */
        onMouseOver() {
            const refTd = this.$refs.td;
            if (refTd) {
                this.widthTooltip = refTd.offsetWidth - 24;
                this.isShowTooltip = refTd.clientWidth < refTd.scrollWidth
            }
        }
    }
}
</script>
<style scoped>
@import './table.css';

.table td:hover [tooltip]::before,
.table td:hover [tooltip]::after {
    display: block;
    opacity: 1;
    transition: opacity 0.6s;
}
</style>
