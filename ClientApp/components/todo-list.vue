<template>
    <div v-if="project">
        <div class="ui fluid input">
            <input v-model="todoTitle"
                v-on:keyup.enter="submit"
                type="text"
                placeholder="Todo Title">
        </div>
        <div class="ui divider"></div>
        <!-->
        <div v-for="todo in todos">
            <todo-item :todo="todo"></todo-item>
        </div>
        <!-->
        <todo-item v-for="todo in project.TodoItems" :todo="todo"></todo-item>
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
            projectId: parseInt(this.$route.params.id)
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
                projectID: this.projectId
            }

            await this.$store.dispatch('addTodo', todo)
            this.todoTitle = ''
        }
    },

    computed: {
        todos: function () {
            return this.$store.state.Todo.todos
        },

        user: function () {
            return this.$store.state.user
        },

        project () {
            return this.$store.getters.getProjectById(this.projectId)
        }
    }
    
}
</script>

<style>
</style>
