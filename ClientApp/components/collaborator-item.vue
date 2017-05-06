<template>
    <div class="item">
        <div class="content">
            <div class="header">{{ collaborator.user.email }}</div>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        collaborator: null
    }
}
</script>

<style>
</style>