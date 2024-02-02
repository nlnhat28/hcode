<template>
    <div class="search-box dark">
        <v-input-text
            v-model="innerValue"
            ref="refInput"
            icon="far fa-magnifying-glass"
            hasClear
            :maxLength="255"
            :label="placeholder"
        />
    </div>
</template>
<script>
import { debounce } from 'lodash';
import { t } from '@/i18n/i18n.js'

export default {
    name: 'VSearchBox',
    props: {
        /**
         * Value to bind
         */
        modelValue: {
            type: String,
        },
        /**
         * Placeholder input
         */
        placeholder: {
            type: String,
            default: t('com.search')
        }
    },
    data() {
        return {
            innerValue: this.modelValue
        }
    },
    beforeUnmount() {
        this.blur();
    },
    watch: {
        innerValue() {
            this.onInput()
        },
    },
    methods: {
        /**
         * Handle input change
         * 
         * Author: nlnhat (12/07/2023)
         */
        onInput: debounce(function () {
            this.$emit('update:modelValue', this.innerValue)
        }, 1000),
        /**
         * Focus
         */
        focus() {
            if (this.$refs.refInput && typeof (this.$refs.refInput.focus) == 'function') {
                this.$refs.refInput.focus();
            }
        },
        blur() {
            if (this.$refs.refInput && typeof(this.$refs.refInput.blur) == 'function') {
                this.$refs.refInput.blur();
            }
        },
        /**
         * Handle keydown on input
         * 
         * Author: nlnhat (29/07/2023)
         */
        onKeyDown(event) {
            if (event.keyCode == this.$enums.keyCode.down) {
                event.preventDefault();
                event.stopPropagation();
                this.$emit('emitFocusFirst');
            }
        }
    }
}
</script>
<style>
@import "./search.css";
</style>