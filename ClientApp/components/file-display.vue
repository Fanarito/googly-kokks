<template>
    <div class="link item">
        <i class="folder icon"></i>
        <div class="content">
            <div class="header">{{ folder.name }}</div>
            <div class="list">
                <file-display v-for="folder in folder.folders" :folder="folder"></file-display>
                <a v-for="file in folder.files" @click="displayFile(file)" class="item">
                    <i class="file icon"></i>
                    <div class="content">
                        <div class="header">{{ file.name }}</div>
                    </div>
                </a>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'file-display',

    props: {
        folder: null
    },

    methods: {
        displayFile (file) {
            this.$store.dispatch('selectFile', file)
        }
    }
}
</script>

<style scoped>
</style>
