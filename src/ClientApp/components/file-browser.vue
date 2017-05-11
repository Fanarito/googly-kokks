<template>
    <div class="ui link list">
        <folder-display v-for="folder in folders" :folder="folder"></folder-display>
    </div>
</template>

<script>
import FolderDisplay from 'components/folder-display'

export default {
    components: {
        FolderDisplay
    },

    props: {
        folders: null
    }
}
</script>

<style scoped>
</style>
