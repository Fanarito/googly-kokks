<template>
    <div v-bind:class="{ active: currentlySelected }" class="item">
        <i class="file icon"></i>
        <a @click="displayFile" @contextmenu.prevent="toggleContext" class="content">
            <div class="header">{{ file.name }}</div>
        </a>

        <context-menu v-if="file == contextMenuObject">
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
        </context-menu>
    </div>
</template>

<script>
import DialogConfirm from 'components/dialog-confirm'
import DialogInput from 'components/dialog-input'
import ContextMenu from 'components/context-menu'

export default {
    components: {
        DialogConfirm,
        DialogInput,
        ContextMenu
    },

    props: {
        file: null
    },

    data () {
        return {
            projectID: parseInt(this.$route.params.id)
        }
    },

    computed: {
        currentlySelected () {
            if (this.$store.state.Project.currentFile) {
                return this.$store.state.Project.currentFile.id === this.file.id
            }
            return false
        },

        contextMenuObject () {
            return this.$store.state.Project.contextObject
        }
    },

    methods: {
        displayFile () {
            this.$store.dispatch('selectFile', { file: this.file, projectID: this.projectID })
        },

        toggleContext () {
            if (this.contextMenuObject === this.file) {
                this.$store.dispatch('setContextObject', null)
            } else {
                this.$store.dispatch('setContextObject', this.file)
            }
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
