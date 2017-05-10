<template>
    <div v-if="project">
        <div class="ui fluid input">
            <input v-model="todoTitle"
                v-on:keyup.enter="submit"
                type="text"
                placeholder="Todo Title">
        </div>
        <div class="ui divider"></div>
        <todo-item v-for="todo in project.todoItems" :todo="todo"></todo-item>
    </div>
    <div v-else>
        loading project
    </div>
</template>

<script>
import TodoItem from './todo-item'

export default {
    components: {
        TodoItem
    },

    data () {
        return {
            todoTitle: '',
            projectID: parseInt(this.$route.params.id)
        }
    },
    
    props: {
        TodoItem: null
    },

    methods: {
        async submit () {
            const todo = {
                name: this.todoTitle,
                description: null,
                isComplete: false,
                userID: this.user.id,
                projectID: this.projectID
            }

            await this.$store.dispatch('addTodo', todo)
            this.todoTitle = ''
        }
    },

    computed: {
        user: function () {
            return this.$store.state.user
        },

        project () {
            return this.$store.getters.getProjectById(this.projectID)
        }
    }
}
</script>

<style>
</style>
