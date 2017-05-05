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
                        <input v-model="projectName" placeholder="Name" type="text" />
                    </div>
                </div>
            </div>
            <div class="actions">
                <div class="ui red deny button">
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
    data() {
        return {
            creating: false,
            projectName: ''
        }
    },
    methods: {
        showModal() {
            $('.ui.modal').modal('show');
        },
        async createProject() {
            // Show loader
            this.creating = true;

            let project = {
                name: this.projectName
            };

            // Clear text box
            this.projectName = '';
            // Tell the store to add the project and wait for it to finish
            await this.$store.dispatch('addProject', project);

            // Hide loader
            this.creating = false;
        }
    },
    mounted() {
        $('.ui.modal').modal({
            detachable: false,
            closable: false,
            offset: 600
        });
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