<template>
  <div class="Home">
    <h1>Home Page</h1>
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
        userID: this.$route.params.id,
        json: null
    }
      },
  created () {
    axios({
      method: 'GET',
      url: 'https://api.greetngroup.com/api/profile/' + this.userID,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
        userID: this.userID
      }
    })
      .then(response => (this.json = response.data))
      .catch(e => { this.errorMessage = e.response.data })
  }
}
</script>

<style>
.Profile{
  width: 70%;
  margin: 1px auto;
}
</style>