import Vue from 'vue'

const state = {
    projects: []
};

const mutations = {
    setProjects: (state, { projects }) => {
        state.projects = projects;
    },

    addProject: (state, { project }) => {
        let changed = false;

        state.projects.some(function (item, idx) {
            if (item.id == project.id) {
                Vue.set(state.projects, idx, project);
                changed = true;
                return true;
            }
            return false;
        });

        if (!changed) {
            state.projects.push(project);
        }
    },

    deleteProject: (state, { project }) => {
        let index = state.projects.indexOf(project);

        if (index > -1) {
            state.projects.splice(index, 1);
        }
    }
};

const actions = {
    async getAllProjects ({ commit }) {
        let response = await Vue.prototype.$http.get('/api/project');
        console.log(response);

        if (response.status === 200) {
            await commit('setProjects', { projects: response.data });
        } else {
            // error handling
        }
    },

    async getProject ({ commit }, id) {
        let response = await Vue.prototype.$http.get('/api/project/' + id);
        console.log(response);

        if (response.status === 200) {
            await commit('addProject', { project: response.data });
        } else {
            // error handling
        }
    },

    async updateProject ({commit, state}, project) {
        let response = await Vue.prototype.$http.put('/api/project/' + project.id, project);
        console.log(response);

        if (response.status === 204) {
            await commit('addProject', { project: project });
        } else {
            // error handling
        }
    },

    async deleteProject ({ commit, state }, project) {
        let response = await Vue.prototype.$http.delete('/api/project/' + project.id, project);
        console.log(response);

        if (response.status === 204) {
            await commit('deleteProject', { project: project });
        } else {
            // error handling
        }
    },

    async addProject ({ commit, state }, project) {
        let response = await Vue.prototype.$http.post('/api/project', project);
        console.log(response);

        if (response.status === 201) {
            await commit('addProject', { project: response.data });
        } else {
            // error handling
        }
    },

    async addCollaborator ({ commit, state }, collaborator) {
        let response = await Vue.prototype.$http.post('/api/collaborator', collaborator);
        console.log(response);

        if (response.status === 201) {
            await commit('addCollaborator', { collaborator: collaborator});
        } else {
            // error handling
        }
    }
};

const getters = {
    getAllProjects: (state, getters) => {
        return state.projects;
    },
    getProjectById: (state, getters) => (id) => {
        return state.projects.find(p => p.id == id);
    }
}

export default {
    state,
    mutations,
    actions,
    getters
};

