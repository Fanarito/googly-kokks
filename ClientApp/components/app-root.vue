<template>
    <div id="app" class="ui grid container">
        <div class="ui row">
            <div class="column">
                <nav-menu params="route: route"></nav-menu>

                <div class="ui segment">
                    <div class="column">
                        <router-view></router-view>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<script>
import Vue from 'vue'
import CounterExample from './counter-example'
import FetchData from './fetch-data'
import HomePage from './home-page'
import NavMenu from './nav-menu'

Vue.component('counter-example', CounterExample);
Vue.component('fetch-data', FetchData);
Vue.component('home-page', HomePage);
Vue.component('nav-menu', NavMenu);

export default {
    data() {
        return {
        }
    }
}
</script>

<style>
</style>