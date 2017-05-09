<template>
    <div>
        <a @click="showModal" class="item">
            New File
            
            <i class="right icons">
                <i class="file icon"></i>
                <i class="corner green plus icon"></i>
            </i>
        </a>
    
        <!-- Dimmer while the project is being created -->
        <div v-if="creating" class="ui fullpage segment">
            <div class="ui active dimmer">
                <div class="ui large indeterminate text loader">Creating file</div>
            </div>
            <p></p>
        </div>
    
        <!-- Popup project creation box -->
        <div class="ui small modal" v-bind:class="[modalClass]">
            <div class="header">Create new file</div>
            <div class="content">
                <div class="ui form">
                    <div class="two fields">
                        <div class="field">
                            <label>Name</label>
                            <input @keyup.enter="createFile" v-model="fileName" placeholder="Name" type="text" />
                        </div>
                        <div class="field">
                            <label>Syntax</label>
                            <select v-model="syntax" class="ui selection dropdown">
                                <option value="0">JavaScript</option>
                                <option value="1">HTML</option>
                                <option value="2">CSS</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <div class="actions">
                <div @click="clearInput" class="ui red deny button">
                    Cancel
                </div>
                <div @click="createFile" class="ui green ok button">
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
            fileName: '',
            projectId: parseInt(this.$route.params.id),
            modalClass: 'fileModal' + this.parent.id,
            syntax: 0
        }
    },
    methods: {
        showModal () {
            $('.ui.modal' + '.' + this.modalClass).modal('show')
        },

        clearInput () {
            this.fileName = ''
        },

        async createFile () {
            // Hide modal
            $('.ui.modal' + '.' + this.modalClass).modal('hide')
            // Hide parent context menu
            this.$emit('hideContext')

            const sameName = this.parent.files.some(f => f.name === this.fileName)
            if (sameName) {
                alert('File by that name already exists, ignoring')
                return
            }

            // Show loader
            this.creating = true

            const file = {
                name: this.fileName,
                parentID: this.parent.id,
                content: '',
                syntax: parseInt(this.syntax)
            }

            // Clear text box
            this.fileName = ''
            // Tell the store to add the project and wait for it to finish
            await this.$store.dispatch('addFile', { file, projectId: this.projectId })
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
