<template>
    <div class="link item">
        <i class="folder icon"></i>
        <div class="content">
            <div @contextmenu.prevent="toggleContext" class="header" v-bind:class="[folderClass]" data-position="bottom left">
                {{ folder.name }}
                <i @click="toggleExpanded" v-bind:class="{ minus: expanded, plus: !expanded }" class="icon"></i>
            </div>
            
            <div v-if="contextMenuVisible" class="ui vertical context menu">
                <file-create-new v-on:hideContext="toggleContext" :parent="folder"></file-create-new>
                <folder-create-new v-on:hideContext="toggleContext" :parent="folder"></folder-create-new>
                <a @click="renameFolder" class="item">
                    Rename Folder

                    <i class="right icons">
                        <i class="folder icon"></i>
                        <i class="corner yellow radio icon"></i>
                    </i>
                </a>
                <confirm-button class="link item" :func="deleteFolder">
                    Delete Folder

                    <i class="right icons">
                        <i class="folder icon"></i>
                        <i class="corner red remove icon"></i>
                    </i>
                </confirm-button>
            </div>

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
import ConfirmButton from 'components/confirm-button'

export default {
    name: 'folder-display',

    components: {
        FileCreateNew,
        FolderCreateNew,
        FileDisplay,
        ConfirmButton
    },

    props: {
        folder: null
    },

    data () {
        return {
            folderClass: 'folderItem' + this.folder.id,
            contextMenuVisible: false,
            expanded: true
        }
    },

    methods: {
        toggleContext () {
            this.contextMenuVisible = !this.contextMenuVisible
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

        async renameFolder () {
            const answer = prompt('Input new name')
            this.toggleContext()

            if (answer.length > 0) {
                await this.$store.dispatch('updateFolder', this.folder)
            }
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
