<template>
  <div class="Login">
    <h1>Logging in</h1>
    <br />
    <br />
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Login',
  data () {
    return {
      email: this.$route.params.email,
      signature: this.$route.params.signature,
      ssoUserId: this.$route.params.ssoUserId,
      timestamp: this.$route.params.timestamp,
    }
      },
  created () {
    axios({
      method: 'GET',
      url: 'https://api.greetngroup.com/api/login',
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
          email: this.email,
          signature: this.signature,
          ssoUserId: this.ssoUserId,
          timestamp: this.timestamp
      }
    })
      .then(response => (localStorage.setItem('JWTToken', response.data)), this.$router.push('/home'))
      .catch(e => (localStorage.setItem('JWTToken', response.data)), this.$router.push('/home'))
  }
}
</script>

<style>
.login{
  width: 70%;
  margin: 1px auto;
}
</style>