<template>
    <div v-if="!confirm" @click.stop="confirm = !confirm">
        <slot class="slot"></slot>
    </div>

    <div v-else>
        <div class="ui small centered header">
            {{ slotText }}
        </div>

        <div class="ui divider"></div>

        <span :class="sureClass">
            Sure?
        </span>

        <div class="ui buttons">
            <div @click.stop="func()" class="ui red button">Yes</div>
            <div class="or"></div>
            <div @click.stop="confirm = !confirm" class="ui button">No</div>
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
            confirm: false,
            slotText: null
        }
    },

    mounted () {
        this.slotText = $(this.$el).text()
    }
}
</script>

