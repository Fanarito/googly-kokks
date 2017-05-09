<template>
    <div class="link item">
        <i class="folder icon"></i>
        <div class="content">
            <div @contextmenu.prevent="showFolderContext" class="header" v-bind:class="[folderClass]" data-position="bottom left">{{ folder.name }}</div>
            
            <div v-if="contextMenuVisible" class="ui vertical menu">
                <file-create-new :parent="folder"></file-create-new>
                <folder-create-new :parent="folder"></folder-create-new>
                <a class="item">
                    <i class="remove icon"></i>
                    Delete Folder
                </a>
            </div>

            <div class="list">
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
            contextMenuVisible: false
        }
    },

    methods: {
        showFolderContext () {
            this.contextMenuVisible = !this.contextMenuVisible
        },

        newFile () {
        }
    },

    mounted () {
        console.log(this.folderClass)
    }
}
</script>

<style scoped>
</style>
