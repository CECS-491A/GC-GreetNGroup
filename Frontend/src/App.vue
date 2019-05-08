<template>
  <v-app>
    <nav-bar />

    <v-content>
      <router-view />
    </v-content>
  </v-app>
</template>

<script>
import NavBar from '@/components/NavBar.vue'
import axios from 'axios'

export default {
  name: 'app',
  components: {
    NavBar
  },
  created () {
    localStorage.setItem('ip', 'N/A')
    axios({method: 'GET', 'url': 'https://httpbin.org/ip'}).then(result => {
      var ipAddr = result.data.origin.split(',')
      localStorage.setItem('ip', ipAddr[0])
    }, error => {
      localStorage.setItem('ip', 'Error')
      console.error(error)
    })
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
