<template>
    <div id="app">
        <nav-menu params="route: route"></nav-menu>
        <div class="ui grid">
            <div class="column">
                <transition name="slide-fade" mode="out-in">
                    <router-view v-if="loaded"></router-view>
                </transition>
            </div>
        </div>
    </div>
</template>

<script>
import NavMenu from 'components/nav-menu'

export default {
    components: {
        NavMenu
    },

    data () {
        return {
            loaded: false
        }
    },

    async created () {
        if (this.$store.getters.currentUser === null) {
            await this.$store.dispatch('getUser')
        }
        this.loaded = true
    }
}
</script>

<style>
.slide-fade-enter-active {
  transition: all .2s ease;
}
.slide-fade-leave-active {
  transition: all .3s cubic-bezier(1.0, 0.5, 0.8, 1.0);
}
.slide-fade-enter, .slide-fade-leave-to
/* .slide-fade-leave-active for <2.1.8 */ {
  transform: translateY(10px);
  opacity: 0;
}
</style>
