<template>
    <div>
        <div @click="showModal" class="ui green labeled icon button">
            <i class="plus icon"></i> New Project
        </div>
    
        <!-- Dimmer while the project is being created -->
        <div v-if="creating" class="ui fullpage segment">
            <div class="ui active dimmer">
                <div class="ui large indeterminate text loader">Preparing Files</div>
            </div>
            <p></p>
        </div>
    
        <!-- Popup project creation box -->
        <div class="ui small modal">
            <div class="header">Create new project</div>
            <div class="content">
                <div class="ui form">
                    <div class="field">
                        <label>Name</label>
                        <input @keyup.enter="createProject" v-model="projectName" placeholder="Name" type="text" />
                    </div>
                </div>
            </div>
            <div class="actions">
                <div @click="clearInput" class="ui red deny button">
                    Cancel
                </div>
                <div @click="createProject" class="ui green ok button">
                    Create
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            creating: false,
            projectName: ''
        }
    },
    methods: {
        showModal () {
            $('.ui.modal').modal('show')
        },
        clearInput () {
            this.projectName = ''
        },
        async createProject () {
            // Hide modal
            $('.ui.modal').modal('hide')
            // Show loader
            this.creating = true

            const project = {
                name: this.projectName
            }

            // Clear text box
            this.projectName = ''
            // Tell the store to add the project and wait for it to finish
            await this.$store.dispatch('addProject', project)
            // Hide loader
            this.creating = false
        }
    }
}
</script>

<style scoped lang="css">
/* Fix for the modal being stuck to bottom */

.modal {
    bottom: auto !important;
}

.ui.fullpage.segment {
    position: fixed;
    top: 0 !important;
    left: 0 !important;
    margin: 0;
    padding: 0;
    border: 0;
    border-radius: 0;
    min-height: 100%;
    min-width: 100%;
    z-index: 1000;
}
</style>
