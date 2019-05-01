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
import { store, isProfileEnabled } from '@/router/request.js'

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
    if (isProfileEnabled === true) {
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
