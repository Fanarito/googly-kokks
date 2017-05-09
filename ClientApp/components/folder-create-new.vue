<template>
    <div>
        <a @click="showModal" class="item">
            New Folder

            <i class="right icons">
                <i class="folder icon"></i>
                <i class="corner green plus icon"></i>
            </i>
        </a>
    
        <!-- Dimmer while the project is being created -->
        <div v-if="creating" class="ui fullpage segment">
            <div class="ui active dimmer">
                <div class="ui large indeterminate text loader">Creating folder</div>
            </div>
            <p></p>
        </div>
    
        <!-- Popup project creation box -->
        <div class="ui small modal" v-bind:class="[modalClass]">
            <div class="header">Create new folder</div>
            <div class="content">
                <div class="ui form">
                    <div class="field">
                        <label>Name</label>
                        <input @keyup.enter="createFolder" v-model="folderName" placeholder="Name" type="text" />
                    </div>
                </div>
            </div>
            <div class="actions">
                <div @click="clearInput" class="ui red deny button">
                    Cancel
                </div>
                <div @click="createFolder" class="ui green ok button">
                    Create
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        parent: null
    },

    data () {
        return {
            creating: false,
            folderName: '',
            projectId: parseInt(this.$route.params.id),
            modalClass: 'folderModal' + this.parent.id
        }
    },
    methods: {
        showModal () {
            $('.ui.modal' + '.' + this.modalClass).modal('show')
        },

        clearInput () {
            this.folderName = ''
        },

        async createFolder () {
            // Hide modal
            $('.ui.modal').modal('hide')
            // Hide parent context menu
            this.$emit('hideContext')

            const sameName = this.parent.folders.some(p => p.name === this.folderName)
            if (sameName) {
                alert('Folder by that name already exists, ignoring')
                return
            }

            // Show loader
            this.creating = true

            const folder = {
                name: this.folderName,
                parentID: this.parent.id,
                projectID: this.projectId,
                folders: [],
                files: []
            }

            // Clear text box
            this.folderName = ''
            // Tell the store to add the project and wait for it to finish
            await this.$store.dispatch('addFolder', folder)
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

.right.icons {
    float: right;
}
</style>
