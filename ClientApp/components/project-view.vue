<template>
    <div class="ui grid">
        <div class="four wide column">
            <side-bar :project="project"></side-bar>
        </div>
        
        <div class="twelve wide column">
            <ace-editor></ace-editor>
        </div>
    </div>
</template>

<script>
import AceEditor from 'components/ace-editor'
import SideBar from 'components/project-sidebar'

export default {
    components: {
        AceEditor,
        SideBar
    },

    data() {
        return {

        }
    },

    computed: {
        project() {
            return this.$store.getters.getProjectById(this.$route.params.id)
        },

        currentCollaborator() {
            return this.$store.getters.getCurrentProjectCollaborator(this.project)
        },

        permission() {
            return this.currentCollaborator.permission
        },
    },

    async created() {
        await this.$store.dispatch('getProject', this.$route.params.id)
    }
}
</script>

<style scoped>
</style>
