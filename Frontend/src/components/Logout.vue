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
    axios({
      method: 'POST',
      url: `${apiURL}` + 'user/logout/' + localStorage.getItem('token'),
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
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
}
</script>
