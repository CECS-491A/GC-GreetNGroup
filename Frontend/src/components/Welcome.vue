<template>
  <div class="Welcome">
    <h1>Welcome to GreetNGroup</h1>
    <h3>Greet people with your interests. Group up to do what you love.</h3>
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
