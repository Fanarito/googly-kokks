<template>
    <div class="card">
        <div class="content">
            <div class="header">{{ project.name }}</div>
            <div class="meta">Creator {temp}</div>
            <div class="description">Maybe some test data</div>
        </div>
        <div class="extra content">
            <router-link :to="{ name: 'projectSettings', params: { id: project.id }}" class="right floated">
                <i class="setting icon"></i>
                Settings
            </router-link>
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
</style>