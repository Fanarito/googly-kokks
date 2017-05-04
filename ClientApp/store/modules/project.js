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
        })

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
    getAllProjects ({ commit }) {
        Vue.prototype.$http.get('/api/project').then(response => {
            console.log(response);
            commit('setProjects', { projects: response.data });
        });
    },
    deleteProject ({ commit, state }, project) {
        Vue.prototype.$http.delete('/api/project/' + project.id, project).then(response => {
            console.log(response);
            commit('deleteProject', { project: project });
        });
    },
    addProject ({ commit, state }, project) {
        Vue.prototype.$http.post('/api/project', project).then(response => {
            console.log(response);
            commit('addProject', { project: response.data });
        });
    
    }
};

const getters = {
    getAllProjects: (state, getters) => {
        return state.projects;
    }
}

export default {
    state,
    mutations,
    actions,
    getters
};

