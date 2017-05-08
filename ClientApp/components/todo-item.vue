<template>
    <div class="panel panel-default">
        <div class="panel-body">
            {{ todo.name }}
            <button @click="remove" type="button" class="close" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        todo: null
    },

    data () {
        return {
        }
    },

    methods: {
        remove () {
            this.$store.dispatch('deleteTodo', this.todo)
        }
    }
}
</script>

<style>
</style>
