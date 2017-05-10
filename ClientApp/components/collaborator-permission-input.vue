<template>
    <div class="ui selection dropdown">
        <input :value="value" type="hidden" name="permission">
        <i class="dropdown icon"></i>
        <div class="default text">Permission</div>
        <div class="menu">
            <div class="item" data-value="1">Read/Write</div>
            <div class="item" data-value="2">Read</div>
        </div>
    </div>
</template>

<script>
export default {
    props: ['value'],

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
