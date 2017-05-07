import TodoList from 'components/todo-list'
import ProjectList from 'components/project-list'
import ProjectSettings from 'components/project-settings'
import AceEditor from 'components/ace-editor'

export const routes = [
    { path: '/', redirect: '/home', show: false},
    { path: '/home', component: ProjectList, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/todo', component: TodoList, display: 'Todos', style: 'glyphicon glyphicon-education'},
    { path: '/project/:id', component: AceEditor, name: 'openProject'},
    { path: '/project/:id/settings', component: ProjectSettings, show: false, name: 'projectSettings' }
]