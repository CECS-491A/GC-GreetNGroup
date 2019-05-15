<template>
  <div class="Login">
    
  </div>
</template>

<script>
import { store, isTokenValid } from '@/router/request.js'

export default {
  name: 'Login',
  data () {
    return {
      loading: false,
      profileIsActivated: false,
      isValidToken: false
    }
  },
  created () {
    this.loading = true
    if (isTokenValid(this.$route.query.token)) {
      localStorage.setItem('token', this.$route.query.token)
      store.state.isLogin = true
      store.getEmail()
      console.log('logged in')
      this.$router.push('/')
    }
  }
}
</script>

<style>
.Home{
  width: 70%;
  margin: 1px auto;
}
</style>
