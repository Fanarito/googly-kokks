import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import TodoList from 'components/todo-list'
import ProjectList from 'components/project-list'

export const routes = [
    { path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/todo', component: TodoList, display: 'Todos', style: 'glyphicon glyphicon-education'},
    { path: '/project', component: ProjectList, display: 'Projects', style: 'glyphicon glyphicon-education'}
]