<template>
    <div v-if="project" class="ui container">
        <file-browser :folder="project.folders"></file-browser>
        
        <div class="ui divider"></div>

        <collaborator-list :collaborators="project.collaborators"></collaborator-list>
    </div>

    <div v-else>
        <h2 class="ui header">Fetching project...</h2>
    </div>
</template>

<script>
import FileBrowser from 'components/file-browser'
import CollaboratorList from 'components/collaborator-list'

export default {
    components: {
        FileBrowser,
        CollaboratorList
    },

    props: {
        project: null
    },

    data() {
        return {

        }
    }
}
</script>

<style>
</style>