<template>
    <div class="ui fluid container">
        <div class="ui top attached inverted menu">
            <div @click="saveFile" class="ui link icon item">
                <i class="save icon"></i>
            </div>
            <div class="menu right">
                <router-link :to="{ name: 'projectSettings', params: { id: projectId }}">
                    <i class="setting icon"></i>
                </router-link>
            </div>
        </div>
        <div class="ui bottom attached segment">
            <div id="editor"></div>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            project: null
        },

        data () {
            return {
                editor: null,
                projectId: parseInt(this.$route.params.id)
            }
        },

        computed: {
            file () {
                return this.$store.state.Project.currentFile
            }
        },

        watch: {
            file (val) {
                this.editor.getSession().setValue(val.content)
            }
        },

        methods: {
            saveFile () {
                const updatedFile = this.file
                console.log(updatedFile)
                updatedFile.content = this.editor.getSession().getValue()
                this.$store.dispatch('updateFile', updatedFile)
            }
        },

        mounted () {
            this.editor = ace.edit('editor')
            var modelist = ace.require("ace/ext/modelist")
            this.editor.setTheme('ace/theme/chaos')
            this.editor.getSession().setMode('ace/mode/javascript')
            this.editor.setOptions({
                maxLines: 50,
                minLines: 50
            })
        }
    }
</script>

<style scoped>
    #editor 
    { 
        position: absolute;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
    }
</style>
