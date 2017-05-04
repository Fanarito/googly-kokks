<template>
    <div class="ui pointing menu">
        <router-link v-for="route in routes" :to="route.path" class="item">{{ route.display }}</router-link>
        <div class="right menu">
            <form action="/Account/LogOut" method="post" id="logoutForm" class="item">
                <button type="submit" class="ui very basic button">Log off</button>
            </form>
        </div>
    </div>
</template>

<script>
import { routes } from '../routes'

export default {
    data() {
        return {
            routes
        }
    }
}
</script>

<style>
</style>