<template>
    <div class="ui selection dropdown">
        <input :value="value" type="hidden" name="syntax">
        <i class="dropdown icon"></i>
        <div class="default text">Syntax</div>
        <div class="menu">
            <option class="item" data-value="JavaScript">JavaScript</option>
            <option class="item" data-value="HTML">HTML</option>
            <option class="item" data-value="CSS">CSS</option>
            <option class="item" data-value="CPlusPlus">CPlusPlus</option>
            <option class="item" data-value="CSharp">CSharp</option>
            <option class="item" data-value="Python">Python</option>
        </div>
    </div>
</template>

<script>
export default {
    props: ['value'],

    watch: {
        // When the value changes, update the selected value
        value (val) {
            $(this.$el)
                .dropdown('set selected', val)
        }
    },

    mounted () {
        // Use semantic ui dropdown
        $(this.$el)
            .dropdown()
            .dropdown('setting', 'onChange', (value) => {
                this.$emit('input', value)
            })
    }
}
</script>
