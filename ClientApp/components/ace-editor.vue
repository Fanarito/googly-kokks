<template>
    <div id="editor">
        function foo(items) {
            var x = "All this is syntax highlighted";
            return x;
        }
    </div>
</template>

<script>
    export default {
        data () {
            return {
                editor: null
            }
        },

        mounted () {
            this.editor = ace.edit("editor")
            this.editor.setTheme("ace/theme/chaos")
            this.editor.getSession().setMode("ace/mode/javascript")
            this.editor.setOptions({
                maxLines: 35,
                minLines: 35
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
