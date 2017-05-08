<template>
    <div class="item">
        <div class="right floated content">
            <i v-if="canDelete" @click="removeCollaborator" class="ui red link icon remove user"></i>
        </div>

        <div class="content">
            {{ collaborator.user.email }}
        </div>
    </div>
</template>

<script>
export default {
    props: {
        collaborator: null,
        canDelete: false
    },

    methods: {
        removeCollaborator () {
            this.$store.dispatch('removeCollaborator', this.collaborator)
        }
    }
}
</script>

<style>
</style>
