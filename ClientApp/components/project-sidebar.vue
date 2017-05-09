<template>
    <div v-if="project">
        <div class="ui top attached tabular menu">
            <a class="item active" data-tab="filesTab">Files</a>
            <a class="item" data-tab="todoTab">Todo</a>
        </div>
        <div id="file-browser-segment" class="ui bottom attached tab segment active" data-tab="filesTab">
            <file-browser :folders="project.folders"></file-browser>
        </div>
        <div class="ui bottom attached tab segment" data-tab="todoTab">
            <todo-list></todo-list>
        </div>
        
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
import TodoList from 'components/todo-list'

export default {
    components: {
        FileBrowser,
        CollaboratorList,
        TodoList
    },

    props: {
        project: null
    },

    data () {
        return {

        }
    },

    mounted () {
        $('.menu .item')
        .tab()
        ;
    }
}
</script>

<style scoped>
#file-browser-segment {
    max-height: 500px;
    overflow: auto;
}
</style>
