<template>
    <div class="card">
        <div class="content">
            <router-link :to="{ name: 'openProject', params: { id: project.id }}" class="header">
                {{ project.name }}
            </router-link>
            <div v-if="creator.user" class="meta">Creator {{ creator.user.userName }}</div>
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

    data () {
        return {
        }
    },

    computed: {
        creator () {
            return this.project.collaborators.find(c => c.permission === 'Owner')
        }
    },

    methods: {
        remove () {
            this.$store.dispatch('deleteProject', this.project)
        }
    }
}
</script>

<style>
</style>
