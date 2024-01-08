<template>
    <Codemirror
        class="v-code-editor"
        v-model="innerValue"
        :placeholder="placeholder"
        :autofocus="autofocus"
        :indent-with-tab="config.indentWithTab"
        :tab-size="config.tabSize"
        :extensions="extensions"
        @change="onChange"
        @focus="$emit('focus', $event)"
        @blur="$emit('blur', $event)"
    />
</template>

<script>
import { Codemirror } from 'vue-codemirror'
import { oneDark } from '@codemirror/theme-one-dark';
import { javascript } from '@codemirror/lang-javascript';
import { cpp } from '@codemirror/lang-cpp';
import { java } from '@codemirror/lang-java';
import { python } from '@codemirror/lang-python';
import { php } from '@codemirror/lang-php';

export default {
    name: 'VCodeMirror',
    components: {
        Codemirror
    },
    props: {
        modelValue: {
        },
        placeholder: {
        },
        autofocus: {
        },
        language: {
        },
        config: {
            type: Object,
            default: {
                tabSize: 4,
                indentWithTab: true,
            }
        }
    },
    watch: {
        modelValue() {
            this.innerValue = this.modelValue
        },
        innerValue() {
            this.$emit('update:modelValue', this.innerValue);
        }
    },
    computed: {
        extensions() {
            let extensions = [oneDark, javascript(), cpp(), java(), python(), php()]
            return extensions;
        }
    },
    data() {
        return {
            innerValue: this.modelValue,
        }
    },
    methods: {
        onChange(event) {
            this.$emit('change', event)
        }
    }
}
</script>
<style>
@import './code-mirror.css';
</style>