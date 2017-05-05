import TodoList from 'components/todo-list'
import ProjectList from 'components/project-list'

export const routes = [
    { path: '/', redirect: '/home', show: false},
    { path: '/home', component: ProjectList, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/todo', component: TodoList, display: 'Todos', style: 'glyphicon glyphicon-education'},
]