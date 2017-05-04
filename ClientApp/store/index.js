import Vue from 'vue'
import Vuex from 'vuex'
import Todo from './modules/todo'
import Project from './modules/project'

Vue.use(Vuex)

const state = {
    user: null
};

const mutations = {
    setUser: (state, { user }) => {
        state.user = user;
    }
};

const actions = {
    getUser ({ commit }) {
        Vue.prototype.$http.get('/api/user').then(response => {
            console.log(response);
            commit('setUser', { user: response.data })
        });
    }
};

const getters = {
}

export default new Vuex.Store({
    state,
    mutations,
    actions,
    getters,
    modules: {
        Todo,
        Project
    }
});
