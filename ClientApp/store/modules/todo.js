import Vue from 'vue'

const state = {
    todos: []
}

const mutations = {
    setTodos: (state, { todos }) => {
        state.todos = todos
    },
    addTodo: (state, { todo }) => {
        let changed = false

        state.todos.some(function (item, idx) {
            if (item.id == todo.id) {
                Vue.set(state.todos, idx, todo)
                changed = true
                return true
            }
            return false
        })

        if (!changed) {
            state.todos.push(todo);
        }
    },
    deleteTodo: (state, { todo }) => {
        let index = state.todos.indexOf(todo)

        if (index > -1) {
            state.todos.splice(index, 1)
        }
    }
}

const actions = {
    async getAllTodos ({ commit }) {
        let response = await Vue.prototype.$http.get('/api/todo')
        console.log(response)
        if (response.status === 200) {
            await commit('setTodos', { todos: response.data })
        } else {
            // error handling
        }
    },

    async deleteTodo ({ commit, state }, todo) {
        let response = await Vue.prototype.$http.delete('/api/todo/' + todo.id, todo)
        console.log(response)

        if (response.status === 204) {
            await commit('deleteTodo', { todo: todo })
        } else {
            // error handling
        }
    },

    async addTodo ({ commit, state }, todo) {
        let response = await Vue.prototype.$http.post('/api/todo', todo)
        console.log(response)

        if (response.status === 201) {
            await commit('addTodo', { todo: response.data })
        } else {
            // error msg
        }
    }
}

const getters = {
    getAllTodos: (state, getters) => {
        return state.todos
    }
}

export default {
    state,
    mutations,
    actions,
    getters
}
