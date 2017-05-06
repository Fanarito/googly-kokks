<template>
    <div v-if="project">
        <h1 class="ui header">
            {{ project.name }}
        </h1>

        <!-- Project Settings -->
        <div class="ui segment">
            <h3 class="ui header">Project Settings</h3>

            <div class="ui labeled input">
                <div class="ui label">
                    Project Name
                </div>
                <input v-model="projectName" type="text" placeholder="Project Name">
            </div>

            <div class="ui divider"></div>

            <button @click="updateProject" class="ui primary button">
                Save
            </button>
        </div>

        <!-- Project Collaborators -->
        <div class="ui segment">
            <h3 class="ui header">Project Collaborators</h3>

            <collaborator-list :collaborators="project.collaborators"></collaborator-list>

            <div class="ui divider"></div>

            <div class="ui action input">
                <input v-model="newCollaboratorEmail" type="text" placeholder="Email or nick">
                <select v-model="newCollaboratorPermission" class="ui compact selection dropdown">
                    <option value="1">Read/Write</option>
                    <option value="2">Read</option>
                </select>
                <button @click="addCollaborator" class="ui primary button">Add</button>
            </div>
        </div>
    </div>

    <h1 v-else class="ui header">
        Fetching project...
    </h1>
</template>

<script>
import CollaboratorList from './collaborator-list'

export default {
    components: {
        CollaboratorList
    },

    data() {
        return {
            projectName: '',
            newCollaboratorEmail: '',
            newCollaboratorPermission: 1
        }
    },

    computed: {
        project() {
            return this.$store.getters.getProjectById(this.$route.params.id);
        }
    },

    methods: {
        updateProject() {
            let updatedProject = {
                id: this.project.id,
                name: this.projectName,
                collaborators: this.project.collaborators
            }
            this.$store.dispatch('updateProject', updatedProject);
        },

        async addCollaborator() {
            let response = await this.$http.get('/api/user/email', {
                params: {
                    email: this.newCollaboratorEmail
                }
            });
            console.log(response);

            if (response.status === 200) {
                let newCollaborator = {
                    projectId: this.project.id,
                    userId: response.data.id,
                    permission: this.newCollaboratorPermission
                };
                // add collaborator then fetch project again
                await this.$store.dispatch('addCollaborator', newCollaborator);
                await this.$store.dispatch('getProject', this.$route.params.id);
                // reset email
                this.newCollaboratorEmail = '';
            } else {
                // error handling
            }
        }
    },

    async created() {
        await this.$store.dispatch('getProject', this.$route.params.id);
        this.projectName = this.project.name;
    }
}
</script>

<style>
</style>