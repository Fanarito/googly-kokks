<template>
    <div class="panel panel-default">
        <div class="panel-body">
            {{ project.name }}
            <button @click="remove" type="button" class="close" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        project: null
    },

    data() {
        return {
        }
    },

    methods: {
        remove() {
            this.$store.dispatch('deleteProject', this.project);
        }
    }
}
</script>

<style>
<