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
import NavMenu from './nav-menu'

Vue.component('nav-menu', NavMenu);

export default {
    data() {
        return {}
    }
}
</script>

<style>
</style>