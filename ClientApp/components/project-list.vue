<template>
    <div class="ui container">
        <project-create-new></project-create-new>
        <div class="ui divider"></div>
        <div v-if="projects.length > 0" class="ui cards">
            <project-card v-for="project in projects" :project="project"></project-card>
        </div>
        <div v-else class="ui header">No projects found</div>
    </div>
</template>

<script>
import ProjectCard from './project-card'
import ProjectCreateNew from './project-create-new'

export default {
    components: {
        ProjectCard,
        ProjectCreateNew
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
    }
}
</script>

<style>
</style>