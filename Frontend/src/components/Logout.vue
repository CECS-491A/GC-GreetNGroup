<template>
    <div class="Logout">
        </div>
</template>

<script>
export default {
  name: 'Logout',
  data () {
    return {
    }
  },
  created () {
    this.localStorage.setItem('Token', null)
    this.$router.push('/home')
  }
}
</script>
