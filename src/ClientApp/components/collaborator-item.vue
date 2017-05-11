<template>
    <div class="item">
        <div class="right floated content">
            <permission-input v-if="canDelete" v-model="permission"></permission-input>
            <dialog-confirm v-if="canDelete" :func="removeCollaborator">
                <button class="ui right floated red button">Remove</button>
            </dialog-confirm>
        </div>
        <div class="content">
            <h3 class="header">
                {{ collaborator.user.email }}
            </h3>
        </div>
        <div class="bottom aligned content">
        </div>
    </div>
</template>

<script>
import PermissionInput from './collaborator-permission-input'
import DialogConfirm from './dialog-confirm'

export default {
    components: {
        PermissionInput,
        DialogConfirm
    },

    props: {
        collaborator: null,
        canDelete: false
    },

    data () {
        return {
            permission: this.collaborator.permission
        }
    },

    methods: {
        removeCollaborator () {
            this.$store.dispatch('removeCollaborator', this.collaborator)
        },

        updateCollaborator () {
            const updatedCollaborator = this.collaborator
            updatedCollaborator.permission = this.permission

            this.$store.dispatch('updateCollaborator', updatedCollaborator)
        }
    },

    watch: {
        permission () {
            this.updateCollaborator()
        }
    }
}
</script>

<style>
</style>
