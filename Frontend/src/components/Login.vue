<template>
  <div class="Login">
    <br />
    <v-dialog
      v-model="loading"
      hide-overlay
      persistent
      width="300"
    >
      <v-card
        color="primary"
        dark
      >
        <v-card-text>
          Loading
          <v-progress-linear
            indeterminate
            color="white"
            class="mb-0"
          ></v-progress-linear>
        </v-card-text>
      </v-card>
    </v-dialog>
    <br />
  </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'
import { store } from '@/router/request.js'

export default {
  name: 'Home',
  data () {
    return {
      loading: false,
      profileIsActivated: false,
      isValidToken: false
    }
  },
  created () {
    this.loading = true

    axios({
      method: 'GET',
      url: `${apiURL}/profile/isprofileactivated/` + this.$route.params.token,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => { this.isValidToken = response.data })
      .catch(e => { this.isValidToken = e.data })

    if (this.isTokenValid) {
      localStorage.setItem('token', this.$route.params.token)
      store.state.isLogin = true
      store.getEmail()
      this.$router.push('/')
    }
    this.$router.push('/')
  }
}
</script>

<style>
.Home{
  width: 70%;
  margin: 1px auto;
}
</style>
