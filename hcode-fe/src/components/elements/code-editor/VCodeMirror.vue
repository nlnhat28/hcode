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
import problemEnum from "@/enums/problem-enum";

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
            // let extensions = [oneDark, javascript(), cpp(), java(), python(), php()]

            const enumLanguage = problemEnum.language;
            switch (this.language?.JudgeId) {
                case enumLanguage.c:
                case enumLanguage.cpp:
                    return [oneDark, cpp()];
                case enumLanguage.csharp:
                case enumLanguage.java:
                    return [oneDark, cpp(), java()];
                case enumLanguage.javascript:
                    return [oneDark, javascript()];
                case enumLanguage.python:
                    return [oneDark, python()];
                case enumLanguage.php:
                    return [oneDark, cpp(), php()];
                default:
                    return [oneDark, javascript()];
            }

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