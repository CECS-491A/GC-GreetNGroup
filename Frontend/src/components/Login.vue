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
import { store } from '@/router/store.js'

export default {
  name: 'Home',
  data () {
    return {
      loading: false
    }
  },
  created () {
    this.loading = true
    store.authentication_success(this.$route.param.token, this.$route.param.emailaddress)
    this.$router.push('/welcome')
  }
}
</script>

<style>
.Home{
  width: 70%;
  margin: 1px auto;
}
</style>
