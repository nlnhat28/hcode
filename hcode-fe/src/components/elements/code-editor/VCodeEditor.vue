<template>
    <CodeEditor
        class="v-code-editor"
        width="100%"
        height="100%"
        line-nums
        :display-language="false"
    />
</template>
<script>
import CodeEditor from "simple-code-editor";
import "simple-code-editor/themes/themes.css";

export default {
    name: 'VCodeEditor',
    components: {
        CodeEditor,
    }
}
</script>
<style scoped>
@import './code-editor.css';
</style>