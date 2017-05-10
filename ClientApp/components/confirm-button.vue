<template>
    <div v-if="!confirm" @click.prevent="confirm = !confirm">
        <slot></slot>
    </div>

    <div v-else>
        <span :class="sureClass">
            Sure?
        </span>

        <div class="ui buttons">
            <div @click.prevent="func()" class="ui red button">Yes</div>
            <div class="or"></div>
            <div @click.prevent="confirm = !confirm" class="ui button">No</div>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        sureClass: String,
        func: {
            type: Function,
            required: true
        }
    },

    data () {
        return {
            confirm: false
        }
    }
}
</script>

