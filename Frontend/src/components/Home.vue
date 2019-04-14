<template>
  <div class="Home">
    <h1>Home Page</h1>
    <br />
    <br />
  </div>
</template>

<script>
export default {
  name: 'Home',
  data () {
    return {
      
    }
  },
  created () {
    localStorage.setItem('Token', this.$route.param.token)
  }
}
</script>

<style>
.Home{
  width: 70%;
  margin: 1px auto;
}
</style>
