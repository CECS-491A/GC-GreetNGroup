<template>
    <div class="Logout">
        </div>
</template>

<script>
import axios from 'axios'
import { store } from '@/router/request.js'
import { apiURL } from '@/const.js'
export default {
  name: 'Logout',
  data () {
    return {
      token: ''
    }
  },
  created () {
    if (localStorage.getItem('token') !== null) {
      axios({
        method: 'POST',
        url: `${apiURL}` + '/user/logout/',
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          token: localStorage.getItem('token')
        }
      })
        .then(response => (this.message = response.data))
        .catch(e => { this.errorMessage = e.response.data })
      localStorage.removeItem('token')
      store.state.isLogin = false
      store.state.email = ''
      alert('You have been logged out')
      this.$router.push('/')
    }
    this.$router.push('/')
  }
}
</script>
