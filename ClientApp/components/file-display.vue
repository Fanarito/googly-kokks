<template>
    <div v-bind:class="{ active: currentlySelected }" class="item">
        <i class="file icon"></i>
        <a @click="displayFile(file)" @contextmenu.prevent="toggleContext" class="content">
            <div class="header">{{ file.name }}</div>
        </a>

        <div v-if="contextMenuVisible" class="ui vertical context menu">
            <dialog-input class="link item" :func="renameFile">
                Rename File

                <i class="right icons">
                    <i class="file icon"></i>
                    <i class="corner yellow radio icon"></i>
                </i>
            </dialog-input>
            <dialog-confirm class="link item" :func="deleteFile">
                Delete File

                <i class="right icons">
                    <i class="file icon"></i>
                    <i class="corner red remove icon"></i>
                </i>
            </dialog-confirm>
        </div>
    </div>
</template>

<script>
import DialogConfirm from 'components/dialog-confirm'
import DialogInput from 'components/dialog-input'

export default {
    components: {
        DialogConfirm,
        DialogInput
    },

    props: {
        file: null
    },

    data () {
        return {
            contextMenuVisible: false,
            projectID: parseInt(this.$route.params.id)
        }
    },

    computed: {
        currentlySelected () {
            if (this.$store.state.Project.currentFile) {
                return this.$store.state.Project.currentFile.id === this.file.id
            }
            return false
        }
    },

    methods: {
        displayFile (file) {
            this.$store.dispatch('selectFile', file)
        },

        toggleContext () {
            this.contextMenuVisible = !this.contextMenuVisible
        },

        async deleteFile () {
            this.toggleContext()
            await this.$store.dispatch('deleteFile', { file: this.file, projectID: this.projectID })
        },

        renameFile (name) {
            this.toggleContext()
            const updatedFile = this.file
            updatedFile.name = name
            this.$store.dispatch('updateFile', { file: updatedFile, projectID: this.projectID })
        }
    }
}
</script>

<style scoped>
.active.item {
    background-color: lightblue;
}

.ui.vertical.context.menu {
    position: absolute;
    z-index: 2000;
    left: 0; 
    right: 0; 
    margin-left: auto; 
    margin-right: auto; 
}
</style>
