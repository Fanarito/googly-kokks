import Vue from 'vue'
import Vuex from 'vuex'
import Project from './modules/project'

Vue.use(Vuex)

const state = {
    user: null
}

const mutations = {
    setUser: (state, { user }) => {
        state.user = user
    }
}

const actions = {
    getUser ({ commit }) {
        Vue.prototype.$http.get('/api/user').then(response => {
            console.log(response)
            commit('setUser', { user: response.data })
        })
    },

    async updateUser ({ commit, state }, user) {
        const response = await Vue.prototype.$http.put('/api/user/' + user.id, user)
        console.log(response)

        await commit('addUser', { user: user })
    }
}

const getters = {
    currentUser: (state, getters) => {
        return state.user
    }
}

export default new Vuex.Store({
    state,
    mutations,
    actions,
    getters,
    modules: {
        Project
    }
})
