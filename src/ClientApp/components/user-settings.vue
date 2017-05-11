<template>
    <div v-if="currentUser" class="ui container">
        <h1 class="ui header">
            {{ currentUser.userName }}
        </h1>

        <!-- User Settings -->
        <div class="ui segment">
            <h3 class="ui header">User Settings</h3>

            <div class="ui labeled input">
                <div class="ui label">
                    Name
                </div>
                <input v-model="userName" type="text" placeholder="User Name">
            </div>
            <br>
            <br>
            <div class="ui slider checkbox">
                <input type="checkbox" name="newsletter">
                <label id="dyslexialabel">Dyslexia mode </label>
                (<i class="stop icon" id="dyslexiaicon"></i> colored background)
                
            </div>

            <div class="ui divider"></div>

            <button @click="updateUser" class="ui primary button">
                Save
            </button>
        </div>
    </div>

    <h1 v-else class="ui header">
        Fetching User...
    </h1>
</template>

<script>
import CollaboratorList from './collaborator-list'

export default {
    components: {
        CollaboratorList
    },

    data () {
        return {
            userName: '',
            newCollaboratorEmail: '',
            newCollaboratorPermission: 1,
            projectID: parseInt(this.$route.params.id)
        }
    },

    computed: {
        currentUser () {
            return this.$store.state.user
        }
    },

    methods: {
        updateUser () {
            const updatedUser = {
                id: this.currentUser.id,
                name: this.currentUser.userName
            }
            this.$store.dispatch('updateUser', updatedUser)
        }
    },

    created () {
        this.userName = this.currentUser.userName
    }
}

</script>

<style scoped>

#dyslexiaicon{
    color:#FAFAC8;
}
</style>
