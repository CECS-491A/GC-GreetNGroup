<template>
  <div class="Welcome">
    <h1>Welcome</h1>
    <br />
    <br />
  </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'
export default {
  name: 'Welcome',
  data () {
    return {
    }
  },
  created () {
    if (localStorage.getItem('token') !== null) {
      axios({
        method: 'POST',
        url: `${apiURL}/profile/isprofileactivated/`,
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          token: localStorage.getItem('token')
        }
      })
        .then(response => {
          console.log(response.data)
        })
        .catch(e => {
          if (e.response.status === 403) {
            this.$router.push('/activateprofile')
          }
        })
    }
  }
}
</script>

<style>
.Welcome{
  width: 70%;
  margin: 1px auto;
}
</style>
