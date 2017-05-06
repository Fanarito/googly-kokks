<template>
    <div id="app">
        <nav-menu params="route: route"></nav-menu>
        <div class="ui container grid">
            <div class="column">
                <router-view></router-view>
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