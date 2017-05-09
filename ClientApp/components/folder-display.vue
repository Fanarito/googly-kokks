<template>
    <div class="link item">
        <i class="folder icon"></i>
        <div class="content">
            <div @contextmenu.prevent="toggleFolderContext" class="header" v-bind:class="[folderClass]" data-position="bottom left">
                {{ folder.name }}
                <i @click="toggleExpanded" v-bind:class="{ minus: expanded, plus: !expanded }" class="icon"></i>
            </div>
            
            <div v-if="contextMenuVisible" class="ui vertical context menu">
                <file-create-new v-on:hideContext="toggleFolderContext" :parent="folder"></file-create-new>
                <folder-create-new v-on:hideContext="toggleFolderContext" :parent="folder"></folder-create-new>
                <a class="item">
                    <i class="remove icon"></i>
                    Delete Folder
                </a>
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

export default {
    name: 'folder-display',

    components: {
        FileCreateNew,
        FolderCreateNew,
        FileDisplay
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
        toggleFolderContext () {
            this.contextMenuVisible = !this.contextMenuVisible
        },

        toggleFileContext () {

        },

        toggleExpanded () {
            this.expanded = !this.expanded
        },

        newFile () {
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
