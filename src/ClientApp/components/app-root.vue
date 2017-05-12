<template>
    <div @click="hideContextMenu" id="app">
        <nav-menu params="route: route"></nav-menu>
        <div v-if="loaded" class="ui grid">
            <div class="column">
                <transition name="slide-fade" mode="out-in">
                    <router-view></router-view>
                </transition>
            </div>
        </div>
        <h3 v-else>
            Preparing data...
        </h3>
    </div>
</template>

<script>
import NavMenu from 'components/nav-menu'

export default {
    components: {
        NavMenu
    },

    data () {
        return {
            loaded: false
        }
    },

    methods: {
        hideContextMenu () {
            // Make sure you are not changing the state needlessly
            if (this.$store.state.Project.contextObject !== null) {
                this.$store.dispatch('setContextObject', null)
            }
        },

        setupTodoSocket () {
            const connection = new WebSocketManager.Connection('ws://' + window.location.host + '/todo')
            connection.enableLogging = true
            connection.connectionMethods.onConnected = () => {
                // optional
                console.log('Todo Socket Connected! Connection ID: ' + connection.connectionId)
            }
            connection.connectionMethods.onDisconnected = () => {
                // optional
                console.log('Disconnected!')
            }
            connection.clientMethods['add'] = (id, name, projectID) => {
                const newTodo = {
                    id: parseInt(id),
                    name: name,
                    projectID: parseInt(projectID)
                }
                this.$store.dispatch('addLocalTodo', newTodo)
            }
            connection.clientMethods['delete'] = (id, projectID) => {
                const todo = {
                    id: parseInt(id),
                    projectID: parseInt(projectID)
                }
                this.$store.dispatch('deleteLocalTodo', todo)
            }
            connection.start()
        },

        setupFileSocket () {
            window.fileSocket = new WebSocketManager.Connection('ws://' + window.location.host + '/file')
            window.fileSocket.enableLogging = true
            window.fileSocket.connectionMethods.onConnected = () => {
                // optional
                console.log('File Socket Connected! Connection ID: ' + window.fileSocket.connectionId)
            }
            window.fileSocket.connectionMethods.onDisconnected = () => {
                // optional
                console.log('Disconnected!')
            }
            window.fileSocket.clientMethods['add'] = (id, name, content, syntax, parentID, projectID) => {
                const file = {
                    id: parseInt(id),
                    name: name,
                    content: content,
                    syntax: syntax,
                    parentID: parseInt(parentID)
                }
                this.$store.dispatch('addLocalFile', { file, projectID: parseInt(projectID) })
                this.$store.dispatch('updateCurrentFile', { file, projectID: parseInt(projectID) })
            }
            window.fileSocket.clientMethods['delete'] = (id, parentID, projectID) => {
                const file = {
                    id: parseInt(id),
                    parentID: parseInt(parentID)
                }
                this.$store.dispatch('deleteLocalFile', { file, projectID: parseInt(projectID) })
            }
            window.fileSocket.clientMethods['change'] = (id, parentID, projectID, userID, change) => {
                this.$store.dispatch('setLatestChange', { fileID: parseInt(id), userID, change })
            }
            window.fileSocket.start()
        },

        setupProjectSocket () {
            const projectSocket = new WebSocketManager.Connection('ws://' + window.location.host + '/project')
            projectSocket.enableLogging = true
            projectSocket.connectionMethods.onConnected = () => {
                // optional
                console.log('File Socket Connected! Connection ID: ' + projectSocket.connectionId)
            }
            projectSocket.connectionMethods.onDisconnected = () => {
                // optional
                console.log('Disconnected!')
            }
            projectSocket.clientMethods['add'] = (id) => {
                this.$store.dispatch('getProject', id)
            }
            projectSocket.clientMethods['delete'] = (id, name) => {
                const project = {
                    id: parseInt(id),
                    name
                }
                this.$store.dispatch('deleteLocalProject', project)
            }
            projectSocket.start()
        }
    },

    mounted () {
        // Sadly vue events don't work for this context
        // so we add a listener to the window that closes the
        // context menu when the user presses the escape key anywhere
        var vm = this
        window.addEventListener('keyup', function (event) {
            // If escape was pressed...
            if (event.keyCode === 27) {
                vm.hideContextMenu()
            }
        })

        this.setupTodoSocket()
        this.setupFileSocket()
        this.setupProjectSocket()
    },

    async created () {
        await this.$store.dispatch('getUser')
        this.loaded = true
    }
}
</script>

<style>
.slide-fade-enter-active {
  transition: all .2s ease;
}
.slide-fade-leave-active {
  transition: all .3s cubic-bezier(1.0, 0.5, 0.8, 1.0);
}
.slide-fade-enter, .slide-fade-leave-to
/* .slide-fade-leave-active for <2.1.8 */ {
  transform: translateY(10px);
  opacity: 0;
}
</style>
