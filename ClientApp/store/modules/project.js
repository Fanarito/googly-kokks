import Vue from 'vue'

const state = {
    projects: [],
    currentFile: null
}

function findFolderById (folders, id) {
    for (var i = 0; i < folders.length; i++) {
        if (folders[i].id === id) {
            return folders[i]
        } else if (folders[i].folders && folders[i].folders.length && typeof folders[i].folders === 'object') {
            return findFolderById(folders[i].folders, id)
        }
    }
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
    },

    setCurrentFile: (state, { file }) => {
        state.currentFile = file
    },

    setFile: (state, { file, projectId }) => {
        const index = state.projects.findIndex(p => p.id === projectId)
        const folder = findFolderById(state.projects[index].folders, file.parentID)
        const fileIndex = folder.files.findIndex(f => f.id === file.id)
        Vue.set(folder.files, fileIndex, file)
    },

    addFile: (state, { file, projectId }) => {
        const index = state.projects.findIndex(p => p.id === projectId)

        if (index > -1) {
            const folder = findFolderById(state.projects[index].folders, file.parentID)
            folder.files.push(file)
        }
    },

    addFolder: (state, { folder }) => {
        const index = state.projects.findIndex(p => p.id === folder.projectID)

        if (index > -1) {
            const parentFolder = findFolderById(state.projects[index].folders, folder.parentID)
            parentFolder.folders.push(folder)
        }
    },

    removeFolder: (state, { folder }) => {
        const index = state.projects.findIndex(p => p.id === folder.projectID)

        if (index > -1) {
            const parentFolder = findFolderById(state.projects[index].folders, folder.parentID)
            const folIndex = parentFolder.folders.indexOf(folder)
            parentFolder.folders.splice(folIndex, 1)
        }
    },

    removeFile: (state, { file, projectID }) => {
        const index = state.projects.findIndex(p => p.id === projectID)

        if (index > -1) {
            const parentFolder = findFolderById(state.projects[index].folders, file.parentID)
            const fileIndex = parentFolder.files.indexOf(file)
            parentFolder.files.splice(fileIndex, 1)
        }
    },

    deleteTodo: (state, { todo }) => {
        const project = state.projects.find(p => p.id === todo.projectID)

        if (project) {
            const todoIndex = project.todoItems.indexOf(todo)
            project.todoItems.splice(todoIndex, 1)
        }
    },

    addTodo: (state, { todo }) => {
        const project = state.projects.find(p => p.id === todo.projectID)

        if (project) {
            project.todoItems.push(todo)
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
    },

    async selectFile ({ commit, state }, file) {
        if (file === null) {
            await commit('setCurrentFile', { file: null })
            return
        }

        const response = await Vue.prototype.$http.get('/api/file/' + file.id)
        console.log(response)

        await commit('setCurrentFile', { file: response.data })
    },

    async updateFile ({ commit, state }, { file, projectId }) {
        const response = await Vue.prototype.$http.put('/api/file/' + file.id, file)
        console.log(response)

        await commit('setFile', { file: file, projectId })
    },

    async addFile ({ commit, state }, { file, projectId }) {
        const response = await Vue.prototype.$http.post('/api/file', file)
        console.log(response)

        await commit('addFile', { file: response.data, projectId })
    },

    async addFolder ({ commit, state }, folder) {
        const response = await Vue.prototype.$http.post('/api/folder', folder)
        console.log(response)

        await commit('addFolder', { folder: response.data })
    },

    async deleteFolder ({ commit, state }, folder) {
        const response = await Vue.prototype.$http.delete('/api/folder/' + folder.id)
        console.log(response)

        await commit('removeFolder', { folder })
    },

    async deleteFile ({ commit, state }, { file, projectID }) {
        const response = await Vue.prototype.$http.delete('/api/file/' + file.id)
        console.log(response)

        await commit('removeFile', { file, projectID })
    },

    async deleteTodo ({ commit, state }, todo) {
        const response = await Vue.prototype.$http.delete('/api/todo/' + todo.id, todo)
        console.log(response)

        if (response.status === 204) {
            await commit('deleteTodo', { todo: todo })
        } else {
            // error handling
        }
    },

    async addTodo ({ commit, state }, todo) {
        const response = await Vue.prototype.$http.post('/api/todo', todo)
        console.log(response)

        if (response.status === 201) {
            await commit('addTodo', { todo: response.data })
        } else {
            // error msg
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
    },

    getFirstFileInProject: (state, getters) => (project) => {
        if (!project) {
            return null
        }
        for (let i = 0; i < project.folders.length; i++) {
            if (project.folders[i].files.length > 0) {
                return project.folders[i].files[0]
            }
        }
    }
}

export default {
    state,
    mutations,
    actions,
    getters
}

