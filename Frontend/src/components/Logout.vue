<template>
    <div class="Logout">
        </div>
</template>

<script>
import { store } from '@/router/request.js'
export default {
  name: 'Logout',
  data () {
    return {
      token: ''
    }
  },
  created () {
    localStorage.removeItem('token')
    store.state.isLogin = false
    store.state.email = ''
    alert('You have been logged out')
    this.$router.push('/')
  }
}
</script>
