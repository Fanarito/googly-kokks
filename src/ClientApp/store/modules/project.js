import Vue from 'vue'

const state = {
    projects: [],
    currentFile: null,
    latestChange: {
        fileID: null,
        userID: null,
        change: null
    },
    contextObject: null
}

function findFolderById (folders, id) {
    for (var i = 0; i < folders.length; i++) {
        if (folders[i].id === id) {
            return folders[i]
        } else if (folders[i].folders && folders[i].folders.length && typeof folders[i].folders === 'object') {
            return findFolderById(folders[i].folders, id)
        }
    }
    return null
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
        console.log(project)
        const index = state.projects.findIndex(p => p.id === project.id)
        console.log(index)
        console.log(project.project)

        if (index > -1) {
            state.projects.splice(index, 1)
        }
    },

    addCollaborator: (state, { collaborator }) => {
        const project = state.projects.find(p => p.id === collaborator.projectID)

        let collaboratorIndex = -1
        if (project !== null) {
            collaboratorIndex = project.collaborators.indexOf(collaborator)
        }

        if (collaboratorIndex > -1) {
            Vue.set(project.collaborators, collaboratorIndex, collaborator)
        } else {
            project.collaborators.push(collaborator)
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

    setCurrentFile: (state, { file, projectID }) => {
        const project = state.projects.find(p => p.id === projectID)

        if (project) {
            const folder = findFolderById(project.folders, file.parentID)
            if (folder) {
                state.currentFile = folder.files.find(f => f.id === file.id)
            }
        }
    },

    addFile: (state, { file, projectID }) => {
        const project = state.projects.find(p => p.id === projectID)

        if (project) {
            const folder = findFolderById(project.folders, file.parentID)
            if (folder) {
                const fileIndex = folder.files.findIndex(f => f.id === file.id)
                if (fileIndex > -1) {
                    Vue.set(folder.files, fileIndex, file)
                } else {
                    folder.files.push(file)
                }
            }
        }
    },

    addFolder: (state, { folder }) => {
        const index = state.projects.findIndex(p => p.id === folder.projectID)

        if (index > -1) {
            const parentFolder = findFolderById(state.projects[index].folders, folder.parentID)
            const folderIndex = parentFolder.folders.findIndex(f => f.id === folder.id)
            if (folderIndex > -1) {
                Vue.set(parentFolder.folders, folderIndex, folder)
            } else {
                parentFolder.folders.push(folder)
            }
        }
    },

    removeFolder: (state, { folder }) => {
        const index = state.projects.findIndex(p => p.id === folder.projectID)

        if (index > -1) {
            const parentFolder = findFolderById(state.projects[index].folders, folder.parentID)
            if (parentFolder) {
                const folIndex = parentFolder.folders.indexOf(folder)
                if (folIndex > -1) {
                    parentFolder.folders.splice(folIndex, 1)
                }
            }
        }
    },

    removeFile: (state, { file, projectID }) => {
        const index = state.projects.findIndex(p => p.id === projectID)

        if (index > -1) {
            const parentFolder = findFolderById(state.projects[index].folders, file.parentID)
            if (parentFolder) {
                const fileIndex = parentFolder.files.findIndex(f => f.id === file.id)
                if (fileIndex > -1) {
                    parentFolder.files.splice(fileIndex, 1)
                }
            }
        }
    },

    deleteTodo: (state, { todo }) => {
        const project = state.projects.find(p => p.id === todo.projectID)

        if (project) {
            const todoIndex = project.todoItems.indexOf(todo)
            if (todoIndex > -1) {
                project.todoItems.splice(todoIndex, 1)
            }
        }
    },

    addTodo: (state, { todo }) => {
        const project = state.projects.find(p => p.id === todo.projectID)

        if (project) {
            const exists = project.todoItems.find(t => t.id === todo.id)
            if (!exists) {
                project.todoItems.push(todo)
            }
        }
    },

    setContextObject: (state, { object }) => {
        state.contextObject = object
    },

    setLatestChange: (state, { fileID, userID, change }) => {
        state.latestChange = {
            fileID,
            userID,
            change
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
            await commit('addProject', { project })
        } else {
            // error handling
        }
    },

    async deleteProject ({ commit, state }, project) {
        const response = await Vue.prototype.$http.delete('/api/project/' + project.id)
        console.log(response)

        if (response.status === 204) {
            await commit('deleteProject', { project })
        } else {
            // error handling
        }
    },

    deleteLocalProject ({ commit, state }, project) {
        commit('deleteProject', { project })
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

    addLocalProject ({ commit, state }, project) {
        commit('addProject', { project })
    },

    async addCollaborator ({ commit, state }, collaborator) {
        const response = await Vue.prototype.$http.post('/api/collaborator', collaborator)
        console.log(response)

        if (response.status === 201) {
            await commit('addCollaborator', { collaborator: response.data })
        } else {
            // error handling
        }
    },

    async updateCollaborator ({ commit, state }, collaborator) {
        const response = await Vue.prototype.$http.put('/api/collaborator/' + collaborator.id, collaborator)
        console.log(response)

        await commit('addCollaborator', { collaborator })
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

    async selectFile ({ commit, state }, { file, projectID }) {
        if (file === null) {
            await commit('setCurrentFile', { file: null })
            return
        }

        const response = await Vue.prototype.$http.get('/api/file/' + file.id)
        console.log(response)

        await commit('setCurrentFile', { file: response.data, projectID })
    },

    async updateCurrentFile ({ commit, state }, { file, projectID }) {
        if (state.currentFile.id === file.id) {
            await commit('setCurrentFile', { file, projectID })
        }
    },

    async addFile ({ commit, state }, { file, projectID }) {
        const response = await Vue.prototype.$http.post('/api/file', file)
        console.log(response)

        await commit('addFile', { file: response.data, projectID })
    },

    addLocalFile ({ commit, state }, { file, projectID }) {
        commit('addFile', { file: file, projectID })
    },

    async updateFile ({ commit, state }, { file, projectID }) {
        const response = await Vue.prototype.$http.put('/api/file/' + file.id, file)
        console.log(response)

        await commit('addFile', { file, projectID })
    },

    async deleteFile ({ commit, state }, { file, projectID }) {
        const response = await Vue.prototype.$http.delete('/api/file/' + file.id)
        console.log(response)

        await commit('removeFile', { file, projectID })
    },

    deleteLocalFile ({ commit, state }, { file, projectID }) {
        commit('removeFile', { file, projectID })
    },

    async addFolder ({ commit, state }, folder) {
        const response = await Vue.prototype.$http.post('/api/folder', folder)
        console.log(response)

        await commit('addFolder', { folder: response.data })
    },

    async updateFolder ({ commit, state }, folder) {
        const response = await Vue.prototype.$http.put('/api/folder/' + folder.id, folder)
        console.log(response)

        await commit('addFolder', { folder: response.data })
    },

    async deleteFolder ({ commit, state }, folder) {
        const response = await Vue.prototype.$http.delete('/api/folder/' + folder.id)
        console.log(response)

        await commit('removeFolder', { folder })
    },

    async deleteTodo ({ commit, state }, todo) {
        const response = await Vue.prototype.$http.delete('/api/todo/' + todo.id, todo)
        console.log(response)

        if (response.status === 204) {
            await commit('deleteTodo', { todo })
        } else {
            // error handling
        }
    },

    deleteLocalTodo ({ commit, state }, todo) {
        commit('deleteTodo', { todo })
    },

    async addTodo ({ commit, state }, todo) {
        const response = await Vue.prototype.$http.post('/api/todo', todo)
        console.log(response)

        if (response.status === 201) {
            await commit('addTodo', { todo: response.data })
        } else {
            // error msg
        }
    },

    addLocalTodo ({ commit, state }, todo) {
        commit('addTodo', { todo })
    },

    async setContextObject ({ commit, state }, object) {
        await commit('setContextObject', { object })
    },

    setLatestChange ({ commit, state }, { fileID, userID, change }) {
        commit('setLatestChange', { fileID, userID, change })
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

