import TodoList from 'components/todo-list'
import ProjectList from 'components/project-list'
import ProjectSettings from 'components/project-settings'
import UserSettings from 'components/user-settings'
import ProjectView from 'components/project-view'

export const routes = [
    { path: '/', redirect: '/home', show: false },
    { path: '/home', component: ProjectList, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/project/:id', component: ProjectView, name: 'openProject'},
    { path: '/project/:id/settings', component: ProjectSettings, show: false, name: 'projectSettings' },
    { path: '/user/settings', component: UserSettings, show: false, name: 'userSettings' }
]