<template>
    <div>
        <div class="ui middle aligned divided list">
            <collaborator-item v-for="collaborator in collaborators" :collaborator="collaborator"></collaborator-item>
        </div>
    </div>
</template>

<script>
import CollaboratorItem from './collaborator-item'

export default {
    components: {
        CollaboratorItem
    },

    props: {
        collaborators: null
    },

    data() {
        return {

        }
    }
}
</script>

<style>
</style>