<!-- <template>
    <AceEditor
        v-model="content"
        @init="editorInit"
        lang="javascript"
        theme="monokai"
        width="100%"
        height="200px"
        :options="{
            enableBasicAutocompletion: true,
            enableLiveAutocompletion: true,
            fontSize: 14,
            highlightActiveLine: true,
            enableSnippets: true,
            showLineNumbers: true,
            tabSize: 2,
            showPrintMargin: false,
            showGutter: true,
        }"
        :commands="[
            {
                name: 'save',
                bindKey: { win: 'Ctrl-s', mac: 'Command-s' },
                exec: dataSumit,
                readOnly: true,
            },
        ]"
    />
</template>
<script>
import AceEditor from "vuejs-ace-editor";

export default {
    name: 'VAceEditor',
    components: {
        AceEditor,
    },
    data() {
        return {
            content: 'const var a = function(){\n}'
        }
    },
    methods: {
        editorInit: function () {
            require('brace/ext/language_tools') //language extension prerequsite...
            require('brace/mode/html')
            require('brace/mode/javascript')    //language
            require('brace/mode/less')
            require('brace/theme/monokai')
            require('brace/snippets/javascript') //snippet
        }
    }
}
</script>
<style scoped>
/* @import './replace.css'; */
</style> -->