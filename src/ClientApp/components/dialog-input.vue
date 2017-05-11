<template>
    <div v-if="!confirm" @click.stop="confirm = !confirm">
        <slot></slot>
    </div>

    <div v-else class="ui center aligned container">
        <div class="ui small centered header">
            {{ slotText }}
        </div>
        
        <div class="ui divider"></div>

        <div class="ui action input">
            <input @keyup.enter="validate" v-focus type="text" v-model="inputData">
        </div>

        <div v-if="error" class="ui negative message">
            <p>
                Input must contain some characters
            </p>
        </div>

        <div class="ui divider"></div>

        <div class="ui buttons">
            <div @click.stop="validate" class="ui green button">Ok</div>
            <div class="or"></div>
            <div @click.stop="confirm = !confirm" class="ui button">Cancel</div>
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
            inputData: '',
            error: false,
            slotText: null
        }
    },

    methods: {
        validate () {
            if (this.inputData.length > 0) {
                this.error = false
                // Call function passed by parent
                this.func(this.inputData)
            } else {
                this.error = true
            }
        }
    },

    mounted () {
        this.slotText = $(this.$el).text()
    }
}
</script>

