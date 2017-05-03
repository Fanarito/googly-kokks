<template>
    <div>
        <h1 class="ui header">
            Login
        </h1>
        <form method="post" class="form-horizontal" role="form" action="/Account/ExternalLogin">
          <div>
            <p>
              <button type="submit" class="ui google button" name="provider" value="Google" title="Log in using your Google account">
                <i class="google icon">
                </i> Google
              </button>
            </p>
          </div>
          <input name="__RequestVerificationToken" type="hidden" value="CfDJ8K8QQNYbheZAlmqIjnM6sqIcjUVESxFX0D_FRGSKc26nN9i4WwKYDfI44LoM6Df9giwDZhHU78-B8MDfqMFRWSLGnDFo3hyaYJf9dAPcsSnhOnhU0WORjMwFaT2o4snGTMYguiRuIxQ8wHUKWUwbk3M" />
        </form>
    </div>
</template>

<script>
export default {
    data() {
        return {
        }
    },
}
</script>

<style>
<