<template>
    <div class="ui tabular menu">
        
        <router-link v-for="route in routes" v-if="route.show != false" :to="route.path" class="item">{{ route.display }}</router-link>
        <div class="right menu">
            <div class="ui icon top dropdown item">
                <i class="user icon"></i>
                <div class="menu">

                    <router-link :to="{ name: 'userSettings'}" class="link item">
                        Settings
                    </router-link>
                    
                    <form action="/Account/LogOut" method="post" id="logoutForm">
                        <div class="link item" type="submit" onclick="javascript:document.getElementById('logoutForm').submit();">
                            Log out
                        </div>
                    </form>   
                </div>
            </div>
        </div>

    </div>
</template>
        
<script>
import { routes } from '../routes'

export default {
    data () {
        return {
            routes
        }
    },

    mounted () {
        $('.ui.dropdown').dropdown()
    }
}
</script>

<style>
</style>
