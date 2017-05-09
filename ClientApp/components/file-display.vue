<template>
    <a @click="displayFile(file)" @contextmenu.prevent="toggleContext" v-bind:class="{ active: currentlySelected }" class="item">
        <i class="file icon"></i>
        <div class="content">
            <div class="header">{{ file.name }}</div>
        </div>

        <div v-if="contextMenuVisible" class="ui vertical context menu">
            <a @click="confirmDeletion" class="item">
                Delete File

                <i class="right icons">
                    <i class="file icon"></i>
                    <i class="corner red remove icon"></i>
                </i>
            </a>
        </div>
    </a>
</template>

<script>
export default {
    props: {
        file: null
    },

    data () {
        return {
            contextMenuVisible: false,
            projectId: parseInt(this.$route.params.id)
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

        async confirmDeletion () {
            const answer = confirm('Are you sure you want to delete "' + this.file.name + '"?')
            this.toggleContext()

            if (answer) {
                await this.$store.dispatch('deleteFile', { file: this.file, projectID: this.projectId })
            }
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
