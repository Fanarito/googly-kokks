<template>
    <div class="ui fluid container">
        <div class="ui top attached menu">
            <div @click="toggleSidebar" id="sidebarToggler" :class="{ active: sidebarVisible }" class="ui link icon item">
                <i class="sidebar icon"></i>
            </div>
            <div @click="saveFile" class="ui link icon item">
                <i v-bind:class="[savingIcon]" class="icon"></i>
                <span class="pad-left" v-if="saving">
                    Saving...
                </span>
                <span class="pad-left" v-if="recentlySaved">
                    Saved...
                </span>
            </div>
            <div class="right menu">
                <router-link :to="{ name: 'projectSettings', params: { id: projectID }}" class="ui link icon item">
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
            sidebarVisible: false,
            saving: false,
            recentlySaved: false
        }
    },

    computed: {
        projectID () {
            return parseInt(this.$route.params.id)
        },

        currentUser () {
            return this.$store.state.user
        },

        project () {
            return this.$store.getters.getProjectById(this.projectID)
        },

        file () {
            return this.$store.state.Project.currentFile
        },

        currentCollaborator () {
            if (this.project) {
                // Fetch the currentCollaborator for the project
                const collaborator = this.$store.getters.getCurrentProjectCollaborator(this.project)
                // Check permissions and set the editor readonly if needed
                if (collaborator.permission === 'Read') {
                    this.editor.setReadOnly(true)
                } else {
                    this.editor.setReadOnly(false)
                }
                return collaborator
            } else {
                // Assume editor readonly
                this.editor.setReadOnly(true)
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
            case 'Python':
                return 'ace/mode/python'
            case 'CPP':
                return 'ace/mode/c_cpp'
            case 'CSharp':
                return 'ace/mode/csharp'
            default:
                return 'ace/mode/plain_text'
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
            if (val !== null) {
                this.editor.getSession().setValue(val.content)
            }
        },

        // Whenever syntax updated, update ace syntax highlighting
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
            // or if the file has not loaded
            // or if the user has recently saved
            if (this.currentCollaborator.permission === 'Read' || this.file === null || this.recentlySaved === true) {
                return
            }

            this.saving = true
            const updatedFile = this.file
            updatedFile.content = this.editorContent
            await this.$store.dispatch('updateFile', { file: updatedFile, projectID: this.projectID })
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
        }, 1500),

        toggleSidebar () {
            $('.ui.sidebar').sidebar({
                context: $('.ui.bottom.attached.segment.pushable'),
                dimPage: false,
                closable: false
            })
            .sidebar('toggle')
            this.sidebarVisible = !this.sidebarVisible
        },

        startEditor () {
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
        }
    },

    mounted () {
        this.startEditor()
        this.toggleSidebar()
    },

    beforeDestroy () {
        this.$store.dispatch('selectFile', null)
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

    .pad-left {
        padding-left: 5px;
    }
</style>
