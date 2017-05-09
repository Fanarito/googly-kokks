<template>
    <div v-if="project" class="ui vertical sidebar menu">
        <file-browser :folders="project.folders"></file-browser>
        
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
        project: null,
        visible: true
    },

    watch: {
        visible (val) {
            console.log(val)
            /*if (val) {
                $('.ui.sidebar').sidebar('show')
            } else {
                $('.ui.sidebar').sidebar('hide')
            }*/
        }
    },

    data () {
        return {

        }
    },

    mounted () {
        $('.ui.sidebar')
        .sidebar({
            context: $('.ui.bottom.attached.segment.pushable')
        })
        .sidebar('attach events', '#sidebarToggler')
    }
}
</script>

<style>
</style>
