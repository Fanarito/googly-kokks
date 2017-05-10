<template>
    <div v-if="!loading" class="ui grid">      
        <div id="editor-view" class="sixteen wide stretched column">
            <ace-editor></ace-editor>
        </div>
    </div>

    <div v-else>
        Fetching project...
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
            projectId: parseInt(this.$route.params.id),
            loading: true
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
        this.loading = true
        await this.$store.dispatch('getProject', this.projectId)
        this.loading = false
    }
}
</script>

<style scoped>
</style>
