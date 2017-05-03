import Vue from 'vue'

const state = {
    todos: []
};

const mutations = {
    setTodos: (state, { todos }) => {
        state.todos = todos;
    },
    addTodo: (state, { todo }) => {
        let changed = false;

        state.todos.some(function (item, idx) {
            if (item.id == todo.id) {
                Vue.set(state.todos, idx, todo);
                changed = true;
                return true;
            }
            return false;
        })

        if (!changed) {
            state.todos.push(todo);
        }
    },
    deleteTodo: (state, { todo }) => {
        let index = state.todos.indexOf(todo);

        if (index > -1) {
            state.todos.splice(index, 1);
        }
    }
};

const actions = {
    getAllTodos ({ commit }) {
        Vue.prototype.$http.get('/api/todo').then(response => {
            console.log(response);
            commit('setTodos', { todos: response.data });
        });
    },
    deleteTodo ({ commit, state }, todo) {
        Vue.prototype.$http.delete('/api/todo/' + todo.id, todo).then(response => {
            console.log(response);
            commit('deleteTodo', { todo: todo });
        });
    },
    addTodo ({ commit, state }, todo) {
        Vue.prototype.$http.post('/api/todo', todo).then(response => {
            console.log(response);
            commit('addTodo', { todo: response.data });
        });
    
    }
};

const getters = {
    getAllTodos: (state, getters) => {
        return state.todos;
    }
}

export default {
    state,
    mutations,
    actions,
    getters
};
