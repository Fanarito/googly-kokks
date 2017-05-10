<template>
    <div v-if="currentUser">
        <div v-if="collaborators.length > 1" class="ui middle aligned divided list">
            <collaborator-item v-for="collaborator in collaborators"
                               v-if="collaborator.userID !== currentUser.id"
                               :collaborator="collaborator"
                               :canDelete="canRemoveCollaborator && allowDelete">
            </collaborator-item>
        </div>

        <h5 v-else class="ui header">
            No collaborators
        </h5>
    </div>

    <div v-else class="ui segment">
        <div class="ui active inverted dimmer">
            <div class="ui text loader">Loading</div>
        </div>
        <p></p>
    </div>
</template>

<script>
import CollaboratorItem from './collaborator-item'

export default {
    components: {
        CollaboratorItem
    },

    props: {
        collaborators: null,
        allowDelete: false
    },

    computed: {
        currentUser () {
            return this.$store.state.user
        },

        canRemoveCollaborator () {
            const currentCollaborator = this.collaborators.find(c => c.userID === this.currentUser.id)

            if (currentCollaborator.permission === 'Owner') {
                return true
            } else {
                return false
            }
        }
    },

    data () {
        return {

        }
    }
}
</script>

<style>
</style>
