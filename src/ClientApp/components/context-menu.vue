<template>
    <div @click.stop class="ui vertical context menu">
        <slot></slot>

        <div @click="hideContext" class="link item">
            Cancel

            <i class="cancel icon"></i>
        </div>
    </div>
</template>

<script>
export default {
    methods: {
        hideContext () {
            this.$store.dispatch('setContextObject', null)
        }
    }
}
</script>
