<template>
    <div class="link item">
        <i class="folder icon"></i>
        <div class="content">
            <div @contextmenu.prevent="toggleContext" class="header" v-bind:class="[folderClass]" data-position="bottom left">
                {{ folder.name }}
                <i @click="toggleExpanded" v-bind:class="{ minus: expanded, plus: !expanded }" class="icon"></i>
            </div>
            
            <context-menu v-if="contextMenuObject == folder">
                <dialog-input class="link item" :func="createFile">
                    New File

                    <i class="right icons">
                        <i class="file icon"></i>
                        <i class="corner green plus icon"></i>
                    </i>
                </dialog-input>

                <dialog-input class="link item" :func="createFolder">
                    New Folder

                    <i class="right icons">
                        <i class="folder icon"></i>
                        <i class="corner green plus icon"></i>
                    </i>
                </dialog-input>

                <dialog-input class="link item" :func="renameFolder">
                    Rename Folder

                    <i class="right icons">
                        <i class="folder icon"></i>
                        <i class="corner yellow radio icon"></i>
                    </i>
                </dialog-input>

                <dialog-confirm class="link item" :func="deleteFolder">
                    Delete Folder

                    <i class="right icons">
                        <i class="folder icon"></i>
                        <i class="corner red remove icon"></i>
                    </i>
                </dialog-confirm>
            </context-menu>

            <div v-if="expanded" class="list">
                <folder-display v-for="folder in folder.folders" :folder="folder"></folder-display>
                <file-display v-for="file in folder.files" :file="file"></file-display>
            </div>
        </div>
    </div>
</template>

<script>
import FileCreateNew from 'components/file-create-new'
import FolderCreateNew from 'components/folder-create-new'
import FileDisplay from 'components/file-display'
import DialogConfirm from 'components/dialog-confirm'
import DialogInput from 'components/dialog-input'
import ContextMenu from 'components/context-menu'

export default {
    name: 'folder-display',

    components: {
        FileCreateNew,
        FolderCreateNew,
        FileDisplay,
        DialogConfirm,
        DialogInput,
        ContextMenu
    },

    props: {
        folder: null
    },

    data () {
        return {
            folderClass: 'folderItem' + this.folder.id,
            expanded: true
        }
    },

    computed: {
        contextMenuObject () {
            return this.$store.state.Project.contextObject
        }
    },

    methods: {
        toggleContext () {
            if (this.contextMenuObject === this.folder) {
                this.$store.dispatch('setContextObject', null)
            } else {
                this.$store.dispatch('setContextObject', this.folder)
            }
        },

        toggleExpanded () {
            this.expanded = !this.expanded
        },

        newFile () {
        },

        async deleteFolder () {
            this.toggleContext()
            await this.$store.dispatch('deleteFolder', this.folder)
        },

        async renameFolder (name) {
            this.toggleContext()
            const newFolder = this.folder
            newFolder.name = name
            await this.$store.dispatch('updateFolder', newFolder)
        },

        async createFile (name) {
            this.toggleContext()

            const sameName = this.folder.files.some(f => f.name === name)
            if (sameName) {
                alert('File by that name already exists, ignoring')
                return
            }

            const file = {
                name: name,
                parentID: this.folder.id,
                content: '',
                syntax: 0
            }

            // Tell the store to add the project and wait for it to finish
            await this.$store.dispatch('addFile', { file, projectID: this.folder.projectID })
        },

        async createFolder (name) {
            this.toggleContext()

            const sameName = this.folder.folders.some(p => p.name === name)
            if (sameName) {
                alert('Folder by that name already exists, ignoring')
                return
            }

            const folder = {
                name: name,
                parentID: this.folder.id,
                projectID: this.folder.projectID,
                folders: [],
                files: []
            }

            // Tell the store to add the project and wait for it to finish
            await this.$store.dispatch('addFolder', folder)
        }
    }
}
</script>

<style scoped>
.ui.vertical.context.menu {
    position: absolute;
    z-index: 2000;
    left: 0; 
    right: 0; 
    margin-left: auto; 
    margin-right: auto; 
}
</style>
