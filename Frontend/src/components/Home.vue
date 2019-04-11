<template>
  <div class="Login">
    <h1>Home</h1>
    <br />
    <br />
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Home',
  data () {
    return {

    }
      },
  created () {
    axios({
      method: 'GET',
      url: 'https://api.greetngroup.com/api/home',
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
          jwtToken: localStorage.jwtToken
      }
    })
      .then(response => (localStorage.setItem('JWTToken', response.data)), this.$router.push('/home'))
      .catch(e => { this.errorMessage = e.response.data })
  }
}
</script>

<style>
.home{
  width: 70%;
  margin: 1px auto;
}
</style>