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
    localStorage.setItem('Token', null)
    alert('You have been logged out')
    this.$router.push('/')
  }
}
</script>
