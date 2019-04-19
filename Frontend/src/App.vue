<template>
  <v-app>
    <v-toolbar app dark>
      <v-toolbar-title class = "NavigationToolBar">
        <span class="font-weight-light">GreetNGroup</span>
        <v-spacer></v-spacer>
      </v-toolbar-title>
      <div>
          <v-btn to="/Search">Search</v-btn>
          <v-btn to="/HelloWorld">HelloPage</v-btn>
          <v-btn to="/testlogin">Login</v-btn>
          <v-btn to="/analysisdashboard">User Analysis</v-btn>
          <v-btn to="/logout"> Log Out</v-btn>
          <v-btn to="/CreateEvent">Create Event</v-btn>
        </div>
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
