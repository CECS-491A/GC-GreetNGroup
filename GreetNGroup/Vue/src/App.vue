<template>
  <v-app>
    <v-toolbar app dark>
      <v-toolbar-title class = "TestToolBar">
        <span class="font-weight-light">GreetNGroup</span>
      </v-toolbar-title>
    </v-toolbar>
    <div id="app">
      <router-view/>
    </div>
  </v-app>
</template>

<script>
export default {
  name: 'App'
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
