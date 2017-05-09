<template>
    <div class="ui fluid container">
        <div class="ui top attached menu">
            <div @click="toggleSidebar" id="sidebarToggler" class="ui link icon item">
                <i class="sidebar icon"></i>
            </div>
            <div @click="saveFile" class="ui link icon item">
                <i v-bind:class="[savingIcon]" class="icon"></i>
                <span class="pad-right" v-if="saving">
                    Saving
                </span>
                <span class="pad-right" v-if="recentlySaved">
                    Saved...
                </span>
            </div>
            <div class="right menu">
                <router-link :to="{ name: 'projectSettings', params: { id: projectId }}" class="ui link icon item">
                    <i class="setting icon"></i>
                </router-link>
            </div>
        </div>

        <div class="ui bottom attached segment pushable">
            <div v-if="project" class="ui sidebar">
                <div>
                    <side-bar :project="project"></side-bar>
                </div>
            </div>

            <!-- side-bar :project="project" :visible="sidebarVisible"></side-bar-->
            <div class="pusher">
                <div class="ui basic segment">
                    <div @keydown.ctrl.83.stop.prevent="saveFile" id="editor"></div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import FileBrowser from 'components/file-browser'
import CollaboratorList from 'components/collaborator-list'
import SideBar from 'components/project-sidebar'
import _ from 'lodash'

export default {
    components: {
        FileBrowser,
        CollaboratorList,
        SideBar
    },

    data () {
        return {
            editor: null,
            editorContent: '',
            projectId: parseInt(this.$route.params.id),
            sidebarVisible: false,
            saving: false,
            recentlySaved: false
        }
    },

    computed: {
        currentUser () {
            return this.$store.state.user
        },

        file () {
            return this.$store.state.Project.currentFile
        },

        project () {
            return this.$store.getters.getProjectById(this.projectId)
        },

        currentCollaborator () {
            if (this.project) {
                return this.$store.getters.getCurrentProjectCollaborator(this.project)
            } else {
                return null
            }
        },

        syntax () {
            if (!this.file) {
                return 'ace/mode/javascript'
            }
            const syntaxString = this.file.syntax

            switch (syntaxString) {
            case 'JavaScript':
                return 'ace/mode/javascript'
            case 'HTML':
                return 'ace/mode/html'
            case 'CSS':
                return 'ace/mode/css'
            }
        },

        savingIcon () {
            if (this.saving) {
                return 'spinner loading'
            } else {
                return 'save'
            }
        }
    },

    watch: {
        file (val) {
            this.editor.getSession().setValue(val.content)
        },

        currentCollaborator (val) {
            if (val !== null && (val.permission === 'Owner' || val.permission === 'ReadWrite')) {
                this.editor.setReadOnly(false)
            } else {
                this.editor.setReadOnly(true)
            }
        },

        // Whenever syntax update ace syntax highlighting
        syntax (val) {
            this.editor.getSession().setMode(val)
        },

        editorContent () {
            this.debounceSaveFile()
        }
    },

    methods: {
        async saveFile () {
            // If the user does not have write privileges just ignore the save request
            if (this.currentCollaborator.permission === 'Read') {
                return
            }

            this.saving = true
            const updatedFile = this.file
            updatedFile.content = this.editorContent
            await this.$store.dispatch('updateFile', { file: updatedFile, projectId: this.projectId })
            this.saving = false

            // Changes Saving text to Saved...
            this.recentlySaved = true
            const self = this
            setTimeout(function () {
                self.recentlySaved = false
            }, 2000)
        },

        debounceSaveFile: _.debounce(function () {
            this.saveFile()
        }, 2000),

        toggleSidebar () {
            $('.ui.sidebar').sidebar({
                context: $('.ui.bottom.attached.segment.pushable'),
                dimPage: false,
                closable: false
            })
            .sidebar('toggle')
        }
    },

    mounted () {
        this.editor = ace.edit('editor')
        this.editor.setTheme('ace/theme/xcode')
        this.editor.getSession().setMode('ace/mode/javascript')
        this.editor.$blockScrolling = Infinity
        this.editor.setOptions({
            maxLines: 50,
            minLines: 50
        })

        const self = this
        this.editor.on('change', function () {
            self.editorContent = self.editor.getSession().getValue()
        })

        this.toggleSidebar()
    }
}
</script>

<style scoped>
    #editor 
    { 
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
    }

    .ui.bottom.attached.segment {
        min-height: 600px;
    }

    .pad-right {
        padding-left: 5px;
    }
</style>
