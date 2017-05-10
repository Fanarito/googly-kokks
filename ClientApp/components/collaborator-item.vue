<template>
    <div class="item">
        <div class="right floated content">
            <permission-input v-if="canDelete" v-model="permission"></permission-input>
            <button v-if="canDelete" @click="removeCollaborator" class="ui red button">Remove</button>
        </div>
        <div class="middle aligned content">
            <div class="header">
                {{ collaborator.user.email }}
            </div>
        </div>
        <div class="bottom aligned content">
        </div>
    </div>
</template>

<script>
import PermissionInput from './collaborator-permission-input'

export default {
    components: {
        PermissionInput
    },

    props: {
        collaborator: null,
        canDelete: false
    },

    data () {
        return {
            permission: this.permissionToNumber()
        }
    },

    methods: {
        permissionToNumber () {
            switch (this.collaborator.permission) {
            case 'Owner':
                return 0
            case 'ReadWrite':
                return 1
            case 'Read':
                return 2
            }
        },

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
