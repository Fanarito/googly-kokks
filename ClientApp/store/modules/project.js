import Vue from 'vue'

const state = {
    projects: []
}

const mutations = {
    setProjects: (state, { projects }) => {
        state.projects = projects
    },

    addProject: (state, { project }) => {
        let changed = false

        state.projects.some(function (item, idx) {
            if (item.id === project.id) {
                Vue.set(state.projects, idx, project)
                changed = true
                return true
            }
            return false
        })

        if (!changed) {
            state.projects.push(project)
        }
    },

    deleteProject: (state, { project }) => {
        const index = state.projects.findIndex(p => p.id === project.id)

        if (index > -1) {
            state.projects.splice(index, 1)
        }
    },

    removeCollaborator: (state, { collaborator }) => {
        const projectIndex = state.projects.findIndex(p => p.id === collaborator.projectID)

        let collaboratorIndex = -1
        if (projectIndex > -1) {
            collaboratorIndex = state.projects[projectIndex].collaborators.indexOf(collaborator)
        }

        if (collaboratorIndex > -1) {
            state.projects[projectIndex].collaborators.splice(collaboratorIndex, 1)
        }
    }
}

const actions = {
    async getAllProjects ({ commit }) {
        const response = await Vue.prototype.$http.get('/api/project')
        console.log(response)

        if (response.status === 200) {
            await commit('setProjects', { projects: response.data })
        } else {
            // error handling
        }
    },

    async getProject ({ commit }, id) {
        const response = await Vue.prototype.$http.get('/api/project/' + id)
        console.log(response)

        if (response.status === 200) {
            await commit('addProject', { project: response.data })
        } else {
            // error handling
        }
    },

    async updateProject ({ commit, state }, project) {
        const response = await Vue.prototype.$http.put('/api/project/' + project.id, project)
        console.log(response)

        if (response.status === 204) {
            await commit('addProject', { project: project })
        } else {
            // error handling
        }
    },

    async deleteProject ({ commit, state }, project) {
        const response = await Vue.prototype.$http.delete('/api/project/' + project.id)
        console.log(response)

        if (response.status === 204) {
            await commit('deleteProject', { project: project })
        } else {
            // error handling
        }
    },

    async addProject ({ commit, state }, project) {
        const response = await Vue.prototype.$http.post('/api/project', project)
        console.log(response)

        if (response.status === 201) {
            await commit('addProject', { project: response.data })
        } else {
            // error handling
        }
    },

    async addCollaborator ({ commit, state }, collaborator) {
        const response = await Vue.prototype.$http.post('/api/collaborator', collaborator)
        console.log(response)

        if (response.status === 201) {

        } else {
            // error handling
        }
    },

    async removeCollaborator ({ commit, state }, collaborator) {
        const response = await Vue.prototype.$http.delete('/api/collaborator/' + collaborator.id)
        console.log(response)

        if (response.status === 204) {
            await commit('removeCollaborator', { collaborator: collaborator })
        } else {
            // error handling
        }
    }
}

const getters = {
    getAllProjects: (state, getters) => {
        return state.projects
    },

    getProjectById: (state, getters) => (id) => {
        return state.projects.find(p => p.id === id)
    },

    getCurrentProjectCollaborator: (state, getters) => (project) => {
        const currentCollaborator = project.collaborators.find(c => c.userID === getters.currentUser.id)
        return currentCollaborator
    }
}

export default {
    state,
    mutations,
    actions,
    getters
}

