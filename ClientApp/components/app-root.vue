<template>
    <div id="app">
        <nav-menu params="route: route"></nav-menu>
        <div class="ui grid">
            <div class="column">
                <router-view v-if="loaded"></router-view>
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
        return {
            loaded: false
        }
    },

    async created() {
        if (this.$store.getters.currentUser === null) {
            await this.$store.dispatch('getUser');
        }
        this.loaded = true;
    }
}
</script>

<style>
</style>