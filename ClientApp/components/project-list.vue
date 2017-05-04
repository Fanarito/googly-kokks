<template>
    <div>
        <div class="ui fluid input">
            <input v-model="projectName"
                v-on:keyup.enter="submit"
                type="text"
                placeholder="Project Name">
        </div>
        <div class="ui divider"></div>
        <div v-for="project in projects">
            <project-item :project="project"></project-item>
        </div>
    </div>
</template>

<script>
import ProjectItem from './project-item'

export default {
    components: {
        ProjectItem
    },

    data() {
        return {
            projectName: ''
        }
    },

    methods: {
        async submit() {
            let project = {
                name: this.projectName
            };

            this.$store.dispatch('addProject', project);
            this.projectName = '';
        }
    },

    computed: {
        projects: function () {
            return this.$store.state.Project.projects;
        },
        user: function () {
            return this.$store.state.user;
        }
    },

    async created() {
        this.$store.dispatch('getAllProjects');
        this.$store.dispatch('getUser');
    }
}
</script>

<style>
<