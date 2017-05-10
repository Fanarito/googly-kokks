<template>
    <div v-if="project" class="ui container">
        <h1 class="ui header">
            {{ project.name }}
        </h1>

        <router-link :to="{ name: 'openProject', params: { id: project.id }}" class="ui right labeled icon button">
            <i class="right arrow icon"></i>
            Go to project
        </router-link>

        <!-- Project Settings -->
        <div v-if="permission == 'Owner'" class="ui segment">
            <h3 class="ui header">Project Settings</h3>

            <div class="ui labeled input">
                <div class="ui label">
                    Name
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

            <collaborator-list v-if="project.collaborators" :collaborators="project.collaborators" :allowDelete="true"></collaborator-list>

            <div class="ui divider"></div>

            <div v-if="permission == 'Owner'" class="ui form">
                <h4 class="ui dividing header">Add Collaborator</h4>
                <div class="field">
                    <div class="two fields">
                        <div class="field">
                            <label>Email</label>
                            <input @keyup.enter="addCollaborator" v-model="newCollaboratorEmail" type="text" placeholder="Email or nick">
                        </div>
                        <div class="field">
                            <label>Permission</label>
                            <permission-input v-model="newCollaboratorPermission"></permission-input>
                        </div>
                    </div>
                </div>
                <div class="ui negative message" id="colaboratorerror">
                    <div class="header">
                        We're sorry we cannot add this collaborator
                    </div>
                    <p>This collaborator does not exist</p>                    
                </div>
                <button @click="addCollaborator" class="ui primary button">Add</button>
            </div>
        </div>

        <!-- Leave or delete project -->
        <div class="ui segment">
            <button @click="leaveProject" class="ui negative button">Leave Project</button>
        </div>
    </div>

    <h1 v-else class="ui header">
        Fetching project...
    </h1>
</template>

<script>
import CollaboratorList from './collaborator-list'
import PermissionInput from './collaborator-permission-input'

export default {
    components: {
        CollaboratorList,
        PermissionInput
    },

    data () {
        return {
            projectName: '',
            newCollaboratorEmail: '',
            newCollaboratorPermission: 1,
            projectId: parseInt(this.$route.params.id)
        }
    },

    computed: {
        project () {
            return this.$store.getters.getProjectById(this.projectId)
        },

        currentCollaborator () {
            return this.$store.getters.getCurrentProjectCollaborator(this.project)
        },

        permission () {
            return this.currentCollaborator.permission
        }
    },

    methods: {
        updateProject () {
            const updatedProject = {
                id: this.project.id,
                name: this.projectName,
                collaborators: this.project.collaborators
            }
            this.$store.dispatch('updateProject', updatedProject)
        },

        async addCollaborator () {
            document.getElementById('colaboratorerror').style.display = 'none'

            try {
                const response = await this.$http.get('/api/user/email', {
                    params: {
                        email: this.newCollaboratorEmail
                    }
                })
                console.log(response)

                const newCollaborator = {
                    projectId: this.project.id,
                    userId: response.data.id,
                    permission: this.newCollaboratorPermission
                }
                // add collaborator then fetch project again
                await this.$store.dispatch('addCollaborator', newCollaborator)
                await this.$store.dispatch('getProject', this.$route.params.id)
                // reset email
                this.newCollaboratorEmail = ''
            } catch (err) {
                console.log(err)
                // display error message
                document.getElementById('colaboratorerror').style.display = 'block'
            }
        },

        async leaveProject () {
            if (this.permission === 'Owner') {
                this.$store.dispatch('deleteProject', this.project)
            } else {
                this.$store.dispatch('removeCollaborator', this.currentCollaborator)
            }
            this.$router.push('/home')
        }
    },

    async created () {
        await this.$store.dispatch('getProject', this.projectId)
        this.projectName = this.project.name
    }
}
</script>

<style scoped>

#colaboratorerror 
{
    display: none;
}
</style>
