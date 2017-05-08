<template>
  <div class="ui link list">
      <file-display v-for="folder in folders" :folder="folder"></file-display>
  </div>
</template>

<script>
import FileDisplay from 'components/file-display'

export default {
    components: {
        FileDisplay
    },

    props: {
        folders: null
    },

    created () {
        console.log(this.folders)
    }
}
</script>

<style scoped>
</style>
