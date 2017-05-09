<template>
    <a @click="displayFile(file)" v-bind:class="{ active: currentlySelected }" class="item">
        <i class="file icon"></i>
        <div class="content">
            <div class="header">{{ file.name }}</div>
        </div>
    </a>
</template>

<script>
export default {
    props: {
        file: null
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
        }
    }
}
</script>

<style scoped>
    .active.item {
        background-color: lightblue;
    }
</style>
