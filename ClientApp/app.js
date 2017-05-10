import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'

Vue.prototype.$http = axios

sync(store, router)

const app = new Vue({
    store,
    router,
    ...App
})

Vue.directive('focus', {
    // Using v-focus on an input focuses it when rendered
    inserted: function (el) {
        el.focus()
    }
})

export {
    app,
    router,
    store
}
