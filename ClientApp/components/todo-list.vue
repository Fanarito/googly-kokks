<template>
    <div>
        <div class="ui fluid input">
            <input v-model="todoTitle"
                v-on:keyup.enter="submit"
                type="text"
                placeholder="Todo Title">
        </div>
        <div class="ui divider"></div>
        <div v-for="todo in todos">
            <todo-item :todo="todo"></todo-item>
        </div>
    </div>
</template>

<script>
import TodoItem from './todo-item'

export default {
    components: {
        TodoItem
    },

    data() {
        return {
            todoTitle: ''
        }
    },

    methods: {
        async submit() {
            let todo = {
                name: this.todoTitle,
                description: null,
                isComplete: false,
                userId: this.user.id
            };

            await this.$store.dispatch('addTodo', todo);
            this.todoTitle = '';
        }
    },

    computed: {
        todos: function () {
            return this.$store.state.Todo.todos;
        },
        user: function () {
            return this.$store.state.user;
        }
    },

    async created() {
        this.$store.dispatch('getAllTodos');
    }
}
</script>

<style>
</style>