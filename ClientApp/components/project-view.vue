<template>
    <div class="ui grid">      
        <div id="editor-view" class="sixteen wide stretched column">
            <ace-editor></ace-editor>
        </div>
    </div>
</template>

<script>
import AceEditor from 'components/ace-editor'

export default {
    components: {
        AceEditor
    },

    data () {
        return {
            projectId: parseInt(this.$route.params.id)
        }
    },

    computed: {
        project () {
            return this.$store.getters.getProjectById(this.projectId)
        },

        currentCollaborator () {
            return this.$store.getters.getCurrentProjectCollaborator(this.project)
        },

        permission () {
            return this.currentCollaborator.permission
        }
    },

    async created () {
        await this.$store.dispatch('getProject', this.projectId)
    }
}
</script>

<style scoped>
</style>
